using Microsoft.Win32;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DFT.Classes
{
    public static class MenuItemUploadAudio
    {
        public static void InitializePreloadedAudioMenuItems(MenuItem menuItemUploadPreloadedAudio)
        {
            try
            {
                string[] resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
                IEnumerable audioResourceNames = resourceNames.Where(x => x.EndsWith(".mp3") || x.EndsWith(".wav")).OrderBy(x => x);
                foreach (string resourceName in audioResourceNames)
                {
                    string[] fileNameComponents = Path.GetFileName(resourceName).Split('.');
                    string audioName = $"{fileNameComponents[fileNameComponents.Length - 2]}.{fileNameComponents.Last()}";
                    string audioExtension = fileNameComponents.Last();

                    var menuItem = new MenuItem()
                    {
                        Header = audioName,
                        Tag = resourceName
                    };

                    string tempFilePath = Path.Combine(Path.GetTempPath(), $"{audioName}_{DateTime.Now.Ticks}.{audioExtension}");
                    menuItem.Click += async (s, args) =>
                    {
                        using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
                        using FileStream fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write);
                        await stream.CopyToAsync(fileStream);

                        var mainWindow = (MainWindow)Application.Current.MainWindow;
                        mainWindow.UploadedAudio = tempFilePath.Replace("\\\\", "\\");

                        await mainWindow.ChartAudioAsync(tempFilePath);
                    };
                    File.Delete(tempFilePath);

                    menuItemUploadPreloadedAudio.Items.Add(menuItem);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Исключение: " + ex.Message, nameof(InitializePreloadedAudioMenuItems), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static async Task UploadAudio()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Audio Files|*.wav;*.mp3"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                string audioFilePath = openFileDialog.FileName;
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.UploadedAudio = openFileDialog.FileName;
                await mainWindow.ChartAudioAsync(audioFilePath);
            }
        }
    }
}