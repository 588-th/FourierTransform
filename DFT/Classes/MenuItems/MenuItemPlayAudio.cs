using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace DFT.Classes
{
    public static class MenuItemPlayAudio
    {
        private static MediaPlayer _player = new MediaPlayer();
        private static MenuItem _menuItemPlayStopAudio;
        private static bool _audioIsPlaying = false;
        private static string _uploadedAudio;

        public static void PlayStopAudio(string uploadedAudio, MenuItem menuItemPlayStopAudio)
        {
            _menuItemPlayStopAudio = menuItemPlayStopAudio;
            _uploadedAudio = uploadedAudio;

            if (uploadedAudio == null)
                return;

            if (_audioIsPlaying)
                AudioStop();
            else
                AudioPlay();
        }

        private static void AudioPlay()
        {
            _menuItemPlayStopAudio.Header = "Stop audio";
            _player.Open(new Uri(_uploadedAudio));
            _player.MediaEnded += PlayerMediaEnded;
            _player.Play();
            _audioIsPlaying = true;
        }

        private static void AudioStop()
        {
            _menuItemPlayStopAudio.Header = "Play audio";
            _player.Stop();
            _audioIsPlaying = false;
        }

        private static void PlayerMediaEnded(object sender, EventArgs e)
        {
            _menuItemPlayStopAudio.Header = "Play audio";
            _audioIsPlaying = false;
        }
    }
}