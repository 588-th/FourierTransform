using DFT.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTestDFT.ExecutionSpeed
{
    [TestClass]
    public class TestExecutionSpeed
    {
        private readonly int _sampleRate = 44100;
        private readonly string _audioDirectory = @"F:\Galanov\GraduationProject\DFT\DFT\AudioFiles\";

        [DataTestMethod]
        [DataRow("KnockKnock_0,5s.mp3")]
        [DataRow("100Hz_5s.mp3")]
        [DataRow("Fire_30s.mp3")]
        [DataRow("Fire_60s.mp3")]
        public async Task TestCalculationSpeedAsync(string audioFileName)
        {
            var stopwatchProject = new Stopwatch();
            stopwatchProject.Start();

            var audioFilePath = Path.Combine(_audioDirectory, audioFileName);

            Console.WriteLine($"Testing audio file: {audioFileName}");

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // Read audio
            var audioData = await ReadAudioDataAsync(audioFileName);
            Console.WriteLine($"Read audio lasted {stopwatch.ElapsedMilliseconds} milliseconds");
            stopwatch.Restart();

            // Calculate amplitude
            var amplitudePoints = CalculateAmplitude(audioData);
            Console.WriteLine($"Amplitude lasted {stopwatch.ElapsedMilliseconds} milliseconds");
            stopwatch.Restart();

            // FFT
            var fftPoints = await CalculateFFTAsync(audioData);
            Console.WriteLine($"FFT lasted {stopwatch.ElapsedMilliseconds} milliseconds");
            stopwatch.Restart();

            // Sine
            double sineDuration = 0.1;
            double frequencySelectionFactor = 0.5;
            var threshold = frequencySelectionFactor * fftPoints.Max(x => x.magnitudes);
            var highMagnitudeFrequencies = fftPoints.Where(x => x.magnitudes >= threshold)
                .Select(x => x.frequencies)
                .ToArray();
            var sinePoints = CalculateSineWave(highMagnitudeFrequencies, sineDuration);
            Console.WriteLine($"Sine lasted {stopwatch.ElapsedMilliseconds} milliseconds");

            // Print overall duration for this audio file
            Console.WriteLine($"Project lasted {stopwatchProject.ElapsedMilliseconds} milliseconds");
        }

        private async Task<float[]> ReadAudioDataAsync(string audioFilePath)
        {
            return await ReadAudioData.ReadAudioAsync(Path.Combine(_audioDirectory, audioFilePath));
        }

        private async Task<List<(double frequencies, double magnitudes)>> CalculateFFTAsync(float[] audioData)
        {
            return await FastFourierTransform.FFTAudioAsync(audioData);
        }

        private List<(double time, double amplitude)> CalculateAmplitude(float[] audioData)
        {
            return Amplitude.CalculateAmplitude(audioData, _sampleRate, true);
        }

        private List<(double time, double amplitude)>[] CalculateSineWave(double[] frequencies, double sineDuration)
        {
            return frequencies.Select(frequency => SineWave.GenerateSineGraph(frequency, sineDuration, _sampleRate)).ToArray();
        }
    }
}