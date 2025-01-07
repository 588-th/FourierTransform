using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

namespace DFT.Classes
{
    public static class FastFourierTransform
    {
        static FastFourierTransform() { }

        /// <summary>
        /// Computes the Fast Fourier Transform (FFT) of the given audio data and returns a list of (frequencies, magnitudes) tuples
        /// that represent the frequency spectrum of the audio data.
        /// </summary>
        /// <param name="audioData">An array of audio samples as floats</param>
        /// <returns>A list of (frequencies, magnitudes) tuples that represent the frequency spectrum of the audio data</returns>
        /// <exception cref="ArgumentException">Thrown if the audio data is null or empty</exception>
        public static async Task<List<(double frequencies, double magnitudes)>> FFTAudioAsync(float[] audioData)
        {
            if (audioData is null || audioData.Length == 0)
            {
                throw new ArgumentException("Audio data cannot be null or empty.", nameof(audioData));
            }

            int n = audioData.Length;
            int paddedLength = (int)Math.Pow(2, Math.Ceiling(Math.Log(n, 2)));
            var complexData = new Complex[paddedLength];

            for (int i = 0; i < n; i++)
            {
                complexData[i] = new Complex(audioData[i], 0);
            }

            BitReverse(complexData, n);

            for (int step = 2; step <= paddedLength; step <<= 1)
            {
                var twiddleFactors = GetTwiddleFactors(step);

                Parallel.For(0, paddedLength / step, j =>
                {
                    int start = j * step;
                    for (int i = 0; i < step / 2; i++)
                    {
                        Complex even = complexData[start + i];
                        Complex odd = complexData[start + i + (step / 2)] * twiddleFactors[i];
                        complexData[start + i] = even + odd;
                        complexData[start + i + (step / 2)] = even - odd;
                    }
                });
            }

            var points = new List<(double frequencies, double magnitudes)>(paddedLength / 2);

            for (int i = 0; i < paddedLength / 2; i++)
            {
                double frequencies = i * 1.0 / paddedLength * 44100;
                double magnitudes = 2 * Math.Sqrt(complexData[i].Real * complexData[i].Real + complexData[i].Imaginary * complexData[i].Imaginary) / paddedLength;
                int scale = 100000;
                points.Add((frequencies, magnitudes * scale));
            }

            return await Task.FromResult(points);
        }

        /// <summary>
        /// Generates an array of complex twiddle factors for the given length.
        /// </summary>
        /// <param name="length">The length of the array of twiddle factors</param>
        /// <returns>An array of complex twiddle factors</returns>
        private static Complex[] GetTwiddleFactors(int length)
        {
            Complex[] twiddleFactors = new Complex[length / 2];
            for (int i = 0; i < length / 2; i++)
            {
                double angle = 2 * Math.PI * i / length;
                twiddleFactors[i] = new Complex(Math.Cos(angle), -Math.Sin(angle));
            }
            return twiddleFactors;
        }

        /// <summary>
        /// Reverses the order of the bits in the indices of the given data array using the bit-reversal algorithm.
        /// This method is used to reorder the data array for the FFT algorithm to work correctly.
        /// </summary>
        /// <param name="data">The data array to be bit-reversed</param>
        /// <param name="n">The size of the data array</param>
        private static void BitReverse(Complex[] data, int n)
        {
            int limit = HighestOneBitIndex(n) + 1;
            for (int i = 0; i < n; i++)
            {
                int j = 0;
                for (int bit = 0; bit < limit; bit++)
                {
                    j |= ((i >> bit) & 1) << (limit - 1 - bit);
                }
                if (j > i)
                {
                    (data[j], data[i]) = (data[i], data[j]);
                }
            }
        }

        /// <summary>
        /// Returns the index of the highest set bit in the binary representation of the given integer.
        /// </summary>
        /// <param name="n">The integer to find the highest set bit of</param>
        /// <returns>The index of the highest set bit, or -1 if the given integer is 0</returns>
        private static int HighestOneBitIndex(int n)
        {
            int result = 0;
            while (n > 0)
            {
                result++;
                n >>= 1;
            }
            return result - 1;
        }
    }
}