using DFT.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTestDFT
{
    [TestClass]
    public class TestFastFourierTransformClass
    {
        private readonly string _audioDirectory = @"F:\Galanov\GraduationProject\DFT\DFT\AudioFiles\";

        [DataTestMethod]
        [DataRow("100Hz_5s.mp3", "100")]
        [DataRow("200Hz_5s.mp3", "200")]
        [DataRow("440Hz_5s.mp3", "440")]
        [DataRow("560Hz_5s.mp3", "560")]
        [DataRow("1000Hz_5s.mp3", "1000")]
        public async Task TestFFT(string audioFileName, string controlResult)
        {
            var audioFilePath = Path.Combine(_audioDirectory, audioFileName);
            var audioData = await ReadAudioData.ReadAudioAsync(Path.Combine(_audioDirectory, audioFilePath));
            var fftPoints = await FastFourierTransform.FFTAudioAsync(audioData);
            var maxMagnitudes = fftPoints.Max(x => x.magnitudes);
            var highMagnitudeFrequencie = fftPoints.Where(x => x.magnitudes == maxMagnitudes).Select(x => x.frequencies).ToArray();
            var result = Math.Round(highMagnitudeFrequencie[0]);
            Assert.AreEqual(controlResult, result.ToString());
        }
    }
}