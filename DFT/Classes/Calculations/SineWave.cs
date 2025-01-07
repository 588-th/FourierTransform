using System;
using System.Collections.Generic;

namespace DFT.Classes
{
    public static class SineWave
    {
        /// <summary>
        /// Generates a list of (time, amplitude) tuples that represent a sine wave graph
        /// with the given frequency, duration, and sample rate.
        /// </summary>
        /// <param name="frequency">The frequency of the sine wave in Hz</param>
        /// <param name="duration">The duration of the sine wave in seconds</param>
        /// <param name="sampleRate">The number of samples per second</param>
        /// <returns>A list of (time, amplitude) tuples that represent a sine wave graph</returns>
        public static List<(double time, double amplitude)> GenerateSineGraph(double frequency, double duration, int sampleRate)
        {
            List<(double, double)> values = new List<(double, double)>();

            int sampleCount = (int)(duration * sampleRate);
            double timeStep = 1.0 / sampleRate;

            for (int i = 0; i < sampleCount; i += 10)
            {
                double time = i * timeStep;
                double amplitude = Math.Sin(2 * Math.PI * frequency * time);
                values.Add((time * 1000, amplitude));
            }

            return values;
        }
    }
}