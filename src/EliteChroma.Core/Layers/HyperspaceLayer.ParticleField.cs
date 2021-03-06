﻿using System;
using System.Collections.Generic;
using Colore.Data;
using EliteChroma.Chroma;

namespace EliteChroma.Core.Layers
{
    internal partial class HyperspaceLayer
    {
        private sealed class ParticleField
        {
            private const double _frontZ = 10;
            private const double _backZ = -10;

            private static readonly Random _rnd = new Random();

            private readonly Queue<Particle> _particles = new Queue<Particle>();

            public void Add(double probability, Color color, double zps)
            {
                while (probability >= 1)
                {
                    _particles.Enqueue(new Particle(color, _frontZ, zps, _rnd.Next(360)));
                    probability--;
                }

                if (_rnd.NextDouble() <= probability)
                {
                    _particles.Enqueue(new Particle(color, _frontZ, zps, _rnd.Next(360)));
                }
            }

            public void MoveAndDraw(TimeSpan deltaT, ChromaCanvas canvas)
            {
                canvas.Keyboard.Set(Color.Black);
                canvas.ChromaLink.Set(Color.Black);

                Trim();

                foreach (var star in _particles)
                {
                    star.MoveAndRender(canvas, deltaT);
                }
            }

            public void Clear()
            {
                _particles.Clear();
            }

            private void Trim()
            {
                while (_particles.Count != 0 && _particles.Peek().Z < _backZ)
                {
                    _particles.Dequeue();
                }
            }
        }
    }
}
