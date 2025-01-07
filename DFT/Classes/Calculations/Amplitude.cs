using System;
using System.Collections.Generic;

namespace DFT.Classes
{
    public class Amplitude
    {
        static Amplitude() { }

        /// <summary>
        /// Calculates the amplitude of audio data over time.
        /// </summary>
        /// <param name="audioData">The audio data to calculate amplitude for.</param>
        /// <param name="sampleRate">The sample rate of the audio data.</param>
        /// <param name="isStereo">Whether the audio data is in stereo format or not.</param>
        /// <returns>A list of (time, amplitude) tuples representing the amplitude of the audio data over time.</returns>
        /// <exception cref="ArgumentException">Thrown when the audio data is null or empty.</exception>
        public static List<(double time, double amplitude)> CalculateAmplitude(float[] audioData, int sampleRate, bool isSterio)
        {
            if (audioData == null || audioData.Length == 0)
            {
                throw new ArgumentException("Audio data cannot be null or empty.", nameof(audioData));
            }

            int numChannels = isSterio ? 2 : 1;
            int sizeFactor = isSterio ? 2 : 4;
            int numSamples = audioData.Length / numChannels;
            double duration = (double)numSamples / sampleRate;
            double timeStep = duration / numSamples;
            List<(double time, double amplitude)> points = new List<(double time, double amplitude)>(numSamples / sizeFactor);

            for (int i = 0; i < numSamples / sizeFactor; i++)
            {
                double time = i * timeStep;
                double amplitude = (audioData[i * numChannels] + audioData[i * numChannels + 1]) / 2;
                points.Add((time, amplitude));
            }

            return points;
        }
    }
}