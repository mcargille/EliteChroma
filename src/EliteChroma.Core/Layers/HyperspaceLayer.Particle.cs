﻿using System;
using System.Diagnostics;
using Colore.Data;
using Colore.Effects.ChromaLink;
using Colore.Effects.Keyboard;
using EliteChroma.Chroma;

namespace EliteChroma.Core.Layers
{
    internal partial class HyperspaceLayer
    {
        private sealed class Particle
        {
            private readonly Color _color;
            private readonly double _zps;
            private readonly int _angle;

            public Particle(Color color, double z0, double zps, int angle)
            {
                _color = color;
                _zps = zps;
                _angle = angle;
                Z = z0;
            }

            public double Z { get; private set; }

            public void MoveAndRender(ChromaCanvas canvas, TimeSpan deltaT)
            {
                var lastZ = Z;
                Z += _zps * deltaT.TotalSeconds;

                var c = _color.Transform(1 / (0.1 + Math.Abs(Z)));

                Render(canvas.Keyboard, c, lastZ);
                Render(canvas.ChromaLink, c);
            }

            private void Render(KeyboardCustom keyboard, Color c, double lastZ)
            {
                // Particles are drawn as stretched lines, simulating movement.
                // Lines are antialiased at both ends.
                //         x0      x1
                //        ┌───┐   ┌───┐
                //  ┌───┬───┬───┬───┬───┬───┐
                //  │   │▒▒▒│███│███│▒▒▒│   │
                //  └───┴───┴───┴───┴───┴───┘
                //   -1   0   1   2   3   4
                const double xScale = KeyboardConstants.MaxColumns / 2.0;
                const double xCenter = (KeyboardConstants.MaxColumns - 1) / 2.0;

                Debug.Assert(lastZ >= Z, "Z always decreases");
                var xPrev = (lastZ > 0 ? Math.Min(1 / lastZ, 2) : 2) * xScale;
                var xCurr = (Z > 0 ? Math.Min(1 / Z, 2) : 2) * xScale;

                double x0, x1;
                if (_angle >= 180)
                {
                    x0 = xCenter + xPrev;
                    x1 = xCenter + xCurr;
                }
                else
                {
                    x1 = xCenter - xPrev;
                    x0 = xCenter - xCurr;
                }

                var y = _angle % KeyboardConstants.MaxRows;

                // First pixel
                var xi = (int)Math.Floor(x0);
                if (xi >= 0 && xi < KeyboardConstants.MaxColumns)
                {
                    var caa = c.Transform(1 - (x0 - xi));
                    keyboard[y, xi] = keyboard[y, xi].Max(caa);
                }

                // Last pixel
                var xj = (int)Math.Ceiling(x1);
                if (xj >= 0 && xj < KeyboardConstants.MaxColumns)
                {
                    var caa = c.Transform(1 - (xj - x1));
                    keyboard[y, xj] = keyboard[y, xj].Max(caa);
                }

                // Middle pixels
                xi = Math.Max(0, xi + 1);
                xj = Math.Min(xj, KeyboardConstants.MaxColumns);
                for (; xi < xj; xi++)
                {
                    keyboard[y, xi] = keyboard[y, xi].Max(c);
                }
            }

            private void Render(ChromaLinkCustom chromaLink, Color c)
            {
                if (_angle > 90)
                {
                    return;
                }

                var i = (_angle % (ChromaLinkConstants.MaxLeds - 1)) + 1;
                chromaLink[i] = chromaLink[i].Max(c);
            }
        }
    }
}
