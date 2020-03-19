﻿using System;
using System.Collections.Generic;
using Colore.Effects.Keyboard;
using EliteFiles.Bindings.Devices;

namespace EliteChroma.Core.Internal
{
    // References:
    // - <EliteRoot>\Products\elite-dangerous-64\ControlSchemes\Help.txt
    // - https://developer.razer.com/works-with-chroma/razer-chroma-led-profiles/
    internal static class KeyMappings
    {
        private static readonly Dictionary<string, Key> _keys = new Dictionary<string, Key>(StringComparer.Ordinal)
        {
            { Keyboard.Escape, Key.Escape },
            { Keyboard.D1, Key.D1 },
            { Keyboard.D2, Key.D2 },
            { Keyboard.D3, Key.D3 },
            { Keyboard.D4, Key.D4 },
            { Keyboard.D5, Key.D5 },
            { Keyboard.D6, Key.D6 },
            { Keyboard.D7, Key.D7 },
            { Keyboard.D8, Key.D8 },
            { Keyboard.D9, Key.D9 },
            { Keyboard.D0, Key.D0 },
            { Keyboard.Minus, Key.OemMinus },
            { Keyboard.EqualsKey, Key.OemEquals },
            { Keyboard.Backspace, Key.Backspace },
            { Keyboard.Tab, Key.Tab },
            { Keyboard.Q, Key.Q },
            { Keyboard.W, Key.W },
            { Keyboard.E, Key.E },
            { Keyboard.R, Key.R },
            { Keyboard.T, Key.T },
            { Keyboard.Y, Key.Y },
            { Keyboard.U, Key.U },
            { Keyboard.I, Key.I },
            { Keyboard.O, Key.O },
            { Keyboard.P, Key.P },
            { Keyboard.Enter, Key.Enter },
            { Keyboard.LeftControl, Key.LeftControl },
            { Keyboard.A, Key.A },
            { Keyboard.S, Key.S },
            { Keyboard.D, Key.D },
            { Keyboard.F, Key.F },
            { Keyboard.G, Key.G },
            { Keyboard.H, Key.H },
            { Keyboard.J, Key.J },
            { Keyboard.K, Key.K },
            { Keyboard.L, Key.L },
            { Keyboard.Apostrophe, Key.OemApostrophe },
            { Keyboard.LeftShift, Key.LeftShift },
            { Keyboard.Z, Key.Z },
            { Keyboard.X, Key.X },
            { Keyboard.C, Key.C },
            { Keyboard.V, Key.V },
            { Keyboard.B, Key.B },
            { Keyboard.N, Key.N },
            { Keyboard.M, Key.M },
            { Keyboard.Comma, Key.OemComma },
            { Keyboard.Period, Key.OemPeriod },
            { Keyboard.RightShift, Key.RightShift },
            { Keyboard.NumpadMultiply, Key.NumMultiply },
            { Keyboard.LeftAlt, Key.LeftAlt },
            { Keyboard.Space, Key.Space },
            { Keyboard.CapsLock, Key.CapsLock },
            { Keyboard.F1, Key.F1 },
            { Keyboard.F2, Key.F2 },
            { Keyboard.F3, Key.F3 },
            { Keyboard.F4, Key.F4 },
            { Keyboard.F5, Key.F5 },
            { Keyboard.F6, Key.F6 },
            { Keyboard.F7, Key.F7 },
            { Keyboard.F8, Key.F8 },
            { Keyboard.F9, Key.F9 },
            { Keyboard.F10, Key.F10 },
            { Keyboard.NumLock, Key.NumLock },
            { Keyboard.ScrollLock, Key.Scroll },
            { Keyboard.Numpad7, Key.Num7 },
            { Keyboard.Numpad8, Key.Num8 },
            { Keyboard.Numpad9, Key.Num9 },
            { Keyboard.NumpadSubtract, Key.NumSubtract },
            { Keyboard.Numpad4, Key.Num4 },
            { Keyboard.Numpad5, Key.Num5 },
            { Keyboard.Numpad6, Key.Num6 },
            { Keyboard.NumpadAdd, Key.NumAdd },
            { Keyboard.Numpad1, Key.Num1 },
            { Keyboard.Numpad2, Key.Num2 },
            { Keyboard.Numpad3, Key.Num3 },
            { Keyboard.Numpad0, Key.Num0 },
            { Keyboard.NumpadDecimal, Key.NumDecimal },
            { Keyboard.Oem102, Key.Invalid },
            { Keyboard.F11, Key.F11 },
            { Keyboard.F12, Key.F12 },
            { Keyboard.F13, Key.Invalid },
            { Keyboard.F14, Key.Invalid },
            { Keyboard.F15, Key.Invalid },
            { Keyboard.Kana, Key.Invalid },
            { Keyboard.AbntC1, Key.Invalid },
            { Keyboard.Convert, Key.Invalid },
            { Keyboard.NoConvert, Key.Invalid },
            { Keyboard.AbntC2, Key.Invalid },
            { Keyboard.PrevTrack, Key.Invalid },
            { Keyboard.Kanji, Key.Invalid },
            { Keyboard.Stop, Key.Invalid },
            { Keyboard.NextTrack, Key.Invalid },
            { Keyboard.RightControl, Key.RightControl },
            { Keyboard.Mute, Key.Invalid },
            { Keyboard.PlayPause, Key.Invalid },
            { Keyboard.VolumeDown, Key.Invalid },
            { Keyboard.VolumeUp, Key.Invalid },
            { Keyboard.WebHome, Key.Invalid },
            { Keyboard.NumpadDivide, Key.NumDivide },
            { Keyboard.SysRQ, Key.PrintScreen },
            { Keyboard.RightAlt, Key.RightAlt },
            { Keyboard.Pause, Key.Pause },
            { Keyboard.Home, Key.Home },
            { Keyboard.UpArrow, Key.Up },
            { Keyboard.PageUp, Key.PageUp },
            { Keyboard.LeftArrow, Key.Left },
            { Keyboard.RightArrow, Key.Right },
            { Keyboard.End, Key.End },
            { Keyboard.DownArrow, Key.Down },
            { Keyboard.PageDown, Key.PageDown },
            { Keyboard.Insert, Key.Insert },
            { Keyboard.Delete, Key.Delete },
            { Keyboard.LeftWin, Key.LeftWindows },
            { Keyboard.RightWin, Key.Invalid },
            { Keyboard.Apps, Key.RightMenu },
            { Keyboard.Sleep, Key.Invalid },
            { Keyboard.WebSearch, Key.Invalid },
            { Keyboard.WebFavourites, Key.Invalid },
            { Keyboard.WebRefresh, Key.Invalid },
            { Keyboard.WebStop, Key.Invalid },
            { Keyboard.WebForward, Key.Invalid },
            { Keyboard.WebBack, Key.Invalid },
            { Keyboard.Mail, Key.Invalid },
        };

        public static IReadOnlyDictionary<string, Key> EliteKeys => _keys;
    }
}
