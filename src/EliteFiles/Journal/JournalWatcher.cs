﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using EliteFiles.Internal;

namespace EliteFiles.Journal
{
    /// <summary>
    /// Watches for changes in Elite:Dangerous player journal files and raises events when new journal entries are recorded.
    /// </summary>
    public sealed class JournalWatcher : IDisposable
    {
        private readonly EliteFileSystemWatcher _journalFilesWatcher;
        private readonly System.Timers.Timer _journalReadTimer;

        private readonly object _journalLock = new object();

        private JournalReader _journalReader;

        private bool _watching;
        private bool _starting;

        /// <summary>
        /// Initializes a new instance of the <see cref="JournalWatcher"/> class
        /// with the given player journal folder path.
        /// </summary>
        /// <param name="journalFolder">The path to the player journal folder.</param>
        public JournalWatcher(string journalFolder)
        {
            if (!Folders.IsValidJournalFolder(journalFolder))
            {
                throw new ArgumentException($"'{journalFolder}' is not a valid Elite:Dangerous journal folder.", nameof(journalFolder));
            }

            _journalFilesWatcher = new EliteFileSystemWatcher(journalFolder, Folders.JournalFilesFilter);
            _journalFilesWatcher.Changed += JournalFilesWatcher_Changed;

            _journalReadTimer = new System.Timers.Timer
            {
                Interval = 250,
                AutoReset = false,
                Enabled = false,
            };

            _journalReadTimer.Elapsed += JournalReadTimer_Elapsed;
        }

        /// <summary>
        /// Occurs when a new entry is read from the player journal.
        /// </summary>
        public event EventHandler<JournalEntry> EntryAdded;

        /// <summary>
        /// Occurs when this instance has finished reading all journal entries
        /// recorded before and during the call to <see cref="Start()"/>.
        /// </summary>
        public event EventHandler<EventArgs> Started;

        /// <summary>
        /// Gets a value indicating whether this instance is done reading
        /// historical journal entries and is watching for new entries.
        /// </summary>
        public bool IsWatching => _watching && !_starting;

        /// <summary>
        /// Starts watching for changes in the player journal.
        /// </summary>
        /// <remarks>
        /// This method will look for the latest journal file and raise the
        /// <see cref="EntryAdded"/> event for each existing entry in that file.
        /// When finished, the <see cref="Started"/> event will be raised.
        /// </remarks>
        public void Start()
        {
            if (_watching)
            {
                return;
            }

            OpenJournal(GetLatestJournalFile());

            _watching = true;
            _starting = true;

            _journalFilesWatcher.Start();
            _journalReadTimer.Start();
        }

        /// <summary>
        /// Stops watching for changes in the player journal.
        /// </summary>
        public void Stop()
        {
            if (!_watching)
            {
                return;
            }

            _watching = false;

            _journalFilesWatcher.Stop();
            _journalReadTimer.Stop();

            CloseJournal();
        }

        /// <summary>
        /// Releases all resources used by the <see cref="JournalWatcher"/>.
        /// </summary>
        public void Dispose()
        {
            Stop();
            _journalFilesWatcher.Dispose();
            _journalReadTimer.Dispose();
        }

        private void JournalFilesWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            OpenJournal(e.FullPath);

            DispatchEventsFromJournal();
        }

        private void JournalReadTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            DispatchEventsFromJournal();

            if (_watching)
            {
                _journalReadTimer.Start();
            }
        }

        private void DispatchEventsFromJournal()
        {
            lock (_journalLock)
            {
                JournalEntry entry;

                while ((entry = _journalReader.ReadEntry()) != null)
                {
                    EntryAdded?.Invoke(this, entry);
                }

                if (_starting)
                {
                    _starting = false;
                    Started?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private void OpenJournal(string path)
        {
            lock (_journalLock)
            {
                Debug.Assert(path != null, "Path cannot be null.");

                if (path.Equals(_journalReader?.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }

                _journalReader?.Dispose();
                _journalReader = new JournalReader(path);
            }
        }

        private void CloseJournal()
        {
            lock (_journalLock)
            {
                _journalReader?.Dispose();
                _journalReader = null;
            }
        }

        private string GetLatestJournalFile()
        {
            var matches =
                from file in Directory.EnumerateFiles(_journalFilesWatcher.Path, _journalFilesWatcher.Filter)
                let filename = Path.GetFileName(file)
                let m = Regex.Match(filename, @"^Journal\.(.+)\.log$", RegexOptions.IgnoreCase)
                where m.Success
                orderby m.Groups[1].Value descending
                select file;

            return matches.FirstOrDefault();
        }
    }
}