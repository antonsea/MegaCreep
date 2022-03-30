using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaCreep
{
    //implements improved Perlin noise in 2D. 
    //Transcribed from http://www.siafoo.net/snippet/144?nolinenos#perlin2003



    public static class PerlinNoise
    {
        private static Random _random = new Random();
        private static int[] _permutation;

        private static Vector2[] _gradients;

        private static float smallestTotal = 0f;
        private static float biggestTotal = 0f;

        static PerlinNoise()
        {

            CalculatePermutation(out _permutation);
            CalculateGradients(out _gradients);
        }

        private static void CalculatePermutation(out int[] p)
        {

            p = Enumerable.Range(0, 256).ToArray();
            //p = new int[] { 1, 6, 9, 10, 2,8,12,19,11,22,33,44,55 };



            /// shuffle the array
            for (var i = 0; i < p.Length; i++)
            {
                var source = _random.Next(p.Length);

                var t = p[i];
                p[i] = p[source];
                p[source] = t;


            }

        }

        /// <summary>
        /// generate a new permutation.
        /// </summary>
        public static void Reseed()
        {
            CalculatePermutation(out _permutation);
            //CalculateGradients(out _gradients);
        }

        private static void CalculateGradients(out Vector2[] grad)
        {

            grad = new Vector2[256];

            for (var i = 0; i < grad.Length; i++)
            {
                Vector2 gradient;

                do
                {
                    gradient = new Vector2((float)(_random.NextDouble() * 2 - 1), (float)(_random.NextDouble() * 2 - 1));
                }
                while (gradient.LengthSquared() >= 1);

                gradient.Normalize();

                grad[i] = gradient;
            }

            for (var i = 0; i < grad.Length; i++)
            {
                var source = _random.Next(grad.Length);

                var t = grad[i];
                grad[i] = grad[source];
                grad[source] = t;


            }

        }

        private static float Drop(float t)
        {
            t = Math.Abs(t);
            return 1f - t * t * t * (t * (t * 6 - 15) + 10);
        }

        private static float Q(float u, float v)
        {

            return Drop(u) * Drop(v);
        }

        private static float smoothStep(float t)
        {
            return t * t * t * (t * (t * 6 - 15) + 10);
            return t * t * (3f - 2 * t);
        }

        private static float lerp(float d1, float d2, float t)
        {
            return d1 + t * (d2 - d1);
        }

        private static float Noise(float x, float y, int output)
        {

            var cell = new Vector2((float)Math.Floor(x), (float)Math.Floor(y));


            var total = 0f;

            var corners = new[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 0), new Vector2(1, 1) };

            int numCycles = 0;
            float[] dotProducts = new float[4];

            foreach (var n in corners)
            {
                var ij = cell + n;
                var uv = new Vector2(x - ij.X, y - ij.Y);


                var index = _permutation[(int)ij.X % _permutation.Length];

                index = _permutation[(index + (int)ij.Y) % _permutation.Length];

                var grad = _gradients[index % _gradients.Length];

                total += Q(uv.X, uv.Y) * Vector2.Dot(grad, uv);


                dotProducts[numCycles] = Vector2.Dot(grad, uv);

                numCycles++;

            }
            

            return Math.Max(Math.Min(total, 1f), -1f);

        }


        public static float[,] GenerateNoiseMap(int width, int height, float frequency, int octaves)
        {
            float[,] data = new float[width, height];

            /// track min and max noise value. Used to normalize the result to the 0 to 1.0 range.
            float min = float.MaxValue;
            float max = float.MinValue;

            /// rebuild the permutation table to get a different noise pattern. 
            /// Leave this out if you want to play with changing the number of octaves while 
            /// maintaining the same overall pattern.
            Reseed();

            float amplitude = 1f;
            //var persistence = 0.25f;

            for (int octave = 0; octave < octaves; octave++)
            {
                /// parallel loop - easy and fast.
                Parallel.For(0
                    , width * height
                    , (offset) =>
                    {

                        int i = offset % width;
                        int j = offset / width;
                        var noise = Noise(i * frequency * 1f / width, j * frequency * 1f / height, offset);
                        noise = data[i, j] += noise * amplitude;

                        min = Math.Min(min, noise);
                        max = Math.Max(max, noise);

                    }
                );

                frequency *= 2;
                amplitude /= 2;

            }

            //Normalize
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    data[x, y] = (data[x, y] - min) / (max - min);
                }
            }


            return data;

        }


    }
}
