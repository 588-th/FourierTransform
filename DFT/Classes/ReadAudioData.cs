using NAudio.Wave;
using System;
using System.Threading.Tasks;

namespace DFT.Classes
{
    /// <summary>
    /// This class implements reading an audio file
    /// </summary>
    public static class ReadAudioData
    {
        static ReadAudioData() { }

        private const int FloatByteSize = sizeof(float);

        /// <summary>
        /// This method implements reading an audio file in asynchronous mode
        /// </summary>
        /// <param name="audioFilePath">Absolute path to the audio file</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<float[]> ReadAudioAsync(string audioFilePath)
        {
            try
            {
                using var audioFileReader = new AudioFileReader(audioFilePath);
                int sampleCount = (int)(audioFileReader.Length / audioFileReader.WaveFormat.Channels);
                float[] audioData = new float[sampleCount];
                int samplesRead = 0;

                while (samplesRead < sampleCount)
                {
                    int remainingSamples = sampleCount - samplesRead;
                    int samplesToRead = Math.Min(remainingSamples, audioData.Length);
                    byte[] byteData = new byte[samplesToRead * FloatByteSize];
                    int samplesReadNow = await audioFileReader.ReadAsync(byteData, 0, byteData.Length);

                    if (samplesReadNow == 0)
                    {
                        break;
                    }

                    int bytesToCopy = samplesReadNow - (samplesReadNow % FloatByteSize);
                    int destinationIndex = samplesRead * FloatByteSize;

                    Buffer.BlockCopy(byteData, 0, audioData, destinationIndex, bytesToCopy);
                    samplesRead += bytesToCopy / FloatByteSize;
                }
                return audioData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading audio file", ex);
            }
        }
    }
}