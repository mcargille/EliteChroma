﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Colore;
using Colore.Data;
using Colore.Effects.ChromaLink;
using Colore.Effects.Headset;
using Colore.Effects.Keyboard;
using Colore.Effects.Keypad;
using Colore.Effects.Mouse;
using Colore.Effects.Mousepad;
using EliteChroma.Chroma;
using Moq;
using Xunit;

namespace EliteChroma.Core.Tests
{
    [SuppressMessage("DocumentationRules", "SA1649:File name should match first type name", Justification = "xUnit test class.")]
    public class ChromaCanvasTest
    {
        [Fact]
        public async Task SetEffectOnlyAppliesAccessedEffects()
        {
            var chroma = new Mock<IChroma> { DefaultValue = DefaultValue.Mock };

            var cc = new ChromaCanvas();
            cc.Keyboard.Set(Color.Blue);

            await cc.SetEffect(chroma.Object).ConfigureAwait(false);

            Mock.Get(chroma.Object.Keyboard)
                .Verify(x => x.SetCustomAsync(It.Is<KeyboardCustom>(y => y[0] == Color.Blue)), Times.Once);
            Mock.Get(chroma.Object.Mouse)
                .Verify(x => x.SetGridAsync(It.IsAny<MouseCustom>()), Times.Never);
            Mock.Get(chroma.Object.Headset)
                .Verify(x => x.SetCustomAsync(It.IsAny<HeadsetCustom>()), Times.Never);
            Mock.Get(chroma.Object.Mousepad)
                .Verify(x => x.SetCustomAsync(It.IsAny<MousepadCustom>()), Times.Never);
            Mock.Get(chroma.Object.Keypad)
                .Verify(x => x.SetCustomAsync(It.IsAny<KeypadCustom>()), Times.Never);
            Mock.Get(chroma.Object.ChromaLink)
                .Verify(x => x.SetCustomAsync(It.IsAny<ChromaLinkCustom>()), Times.Never);
        }

        [Fact]
        public async Task RazerEffectsAreAccessible()
        {
            var chroma = new Mock<IChroma> { DefaultValue = DefaultValue.Mock };

            var cc = new ChromaCanvas();
            cc.Keyboard.Set(Color.Blue);
            cc.Mouse.Set(Color.Green);
            cc.Headset.Set(Color.HotPink);
            cc.Mousepad.Set(Color.Orange);
            cc.Keypad.Set(Color.Pink);
            cc.ChromaLink.Set(Color.Purple);

            await cc.SetEffect(chroma.Object).ConfigureAwait(false);

            Mock.Get(chroma.Object.Keyboard)
                .Verify(x => x.SetCustomAsync(It.Is<KeyboardCustom>(y => y[0] == Color.Blue)));
            Mock.Get(chroma.Object.Mouse)
                .Verify(x => x.SetGridAsync(It.Is<MouseCustom>(y => y[0] == Color.Green)));
            Mock.Get(chroma.Object.Headset)
                .Verify(x => x.SetCustomAsync(It.Is<HeadsetCustom>(y => y[0] == Color.HotPink)));
            Mock.Get(chroma.Object.Mousepad)
                .Verify(x => x.SetCustomAsync(It.Is<MousepadCustom>(y => y[0] == Color.Orange)));
            Mock.Get(chroma.Object.Keypad)
                .Verify(x => x.SetCustomAsync(It.Is<KeypadCustom>(y => y[0] == Color.Pink)));
            Mock.Get(chroma.Object.ChromaLink)
                .Verify(x => x.SetCustomAsync(It.Is<ChromaLinkCustom>(y => y[0] == Color.Purple)));
        }

        [Fact]
        public void SetEffectThrowsOnNullChromaObject()
        {
            var cc = new ChromaCanvas();
            Assert.ThrowsAsync<ArgumentNullException>("chroma", () => cc.SetEffect(null));
        }
    }
}
