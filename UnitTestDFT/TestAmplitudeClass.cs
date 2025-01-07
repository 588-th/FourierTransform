using DFT.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTestDFT
{
    [TestClass]
    public class TestAmplitudeClass
    {
        private readonly string _audioDirectory = @"F:\Galanov\GraduationProject\DFT\DFT\AudioFiles\";

        [DataTestMethod]
        [DataRow("Fire_30s.mp3", "30")]
        [DataRow("Fire_60s.mp3", "60")]
        public async Task TestAudioDuration(string audioFileName, string controlResult)
        {
            var audioFilePath = Path.Combine(_audioDirectory, audioFileName);
            var audioData = await ReadAudioData.ReadAudioAsync(Path.Combine(_audioDirectory, audioFilePath));
            var amplitudePoints = Amplitude.CalculateAmplitude(audioData, 44100, true);
            var audioDuration = Math.Round(amplitudePoints.Last().time, 1);
            Assert.AreEqual(controlResult, audioDuration.ToString());
        }
    }
}
