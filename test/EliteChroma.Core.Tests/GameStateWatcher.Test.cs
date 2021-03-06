using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using EliteChroma.Core.Tests.Internal;
using EliteChroma.Elite;
using Xunit;

namespace EliteChroma.Core.Tests
{
    [SuppressMessage("DocumentationRules", "SA1649:File name should match first type name", Justification = "xUnit test class.")]
    public class GameStateWatcherTest
    {
        private const string _gameRootFolder = @"TestFiles\GameRoot";
        private const string _gameOptionsFolder = @"TestFiles\GameOptions";
        private const string _journalFolder = @"TestFiles\Journal";

        [Fact]
        public void RaisesEventsForEachGameStateChange()
        {
            using var watcher = new GameStateWatcher(_gameRootFolder, _gameOptionsFolder, _journalFolder)
            {
                RaisePreStartupEvents = true,
            };

            var evs = new EventCollector<EventArgs>(h => watcher.Changed += h, h => watcher.Changed -= h);

            evs.Wait(8, watcher.Start, 5000);
            watcher.Stop();
        }

        [Fact]
        public void WatcherDoesNotThrowWhenDisposingTwice()
        {
            var watcher = new GameStateWatcher(_gameRootFolder, _gameOptionsFolder, _journalFolder);
#pragma warning disable IDISP016, IDISP017
            watcher.Dispose();
            watcher.Dispose();
#pragma warning restore IDISP016, IDISP017
        }

        [Fact]
        public void OnChangedIsNotReentrant()
        {
            using var watcher = new GameStateWatcher(_gameRootFolder, _gameOptionsFolder, _journalFolder)
            {
                RaisePreStartupEvents = false,
            };

            var evs = new EventCollector<EventArgs>(h => watcher.Changed += h, h => watcher.Changed -= h);
            evs.Wait(1, watcher.Start, 5000);

            var nOnChangedCalls = 0;
            using var mre = new ManualResetEventSlim();

            watcher.Changed += (sender, e) =>
            {
                Interlocked.Increment(ref nOnChangedCalls);
                mre.Wait();
            };

            void OnChanged()
            {
                watcher.InvokePrivateMethod<object>("OnChanged");
                mre.Set();
            }

            Task.WaitAll(new[]
            {
                Task.Run(OnChanged),
                Task.Run(OnChanged),
            });

            watcher.Stop();

            Assert.Equal(1, nOnChangedCalls);
        }
    }
}
