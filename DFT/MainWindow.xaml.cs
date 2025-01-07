using DFT.Classes;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DFT
{
    public partial class MainWindow : Window
    {
        #region Fields

        public string UploadedAudio;

        private const int SampleRate = 44100;

        private readonly ObservableCollection<ObservablePoint> _amplitudeDataPoints = new ObservableCollection<ObservablePoint>();
        private readonly ObservableCollection<ObservablePoint> _frequencyDataPoints = new ObservableCollection<ObservablePoint>();

        private ISeries[] _amplitudeSeriesCollection;
        private ISeries[] _frequencySeriesCollection;
        private ISeries[] _sineSeriesCollection;

        public Axis[] AmplitudeXAxes { get; } = new[] { new Axis { Name = "Time (s)", NamePaint = new SolidColorPaint(SKColors.White), LabelsPaint = new SolidColorPaint(SKColors.White) } };
        public Axis[] AmplitudeYAxes { get; } = new[] { new Axis { Name = "Amplitude", NamePaint = new SolidColorPaint(SKColors.White), LabelsPaint = new SolidColorPaint(SKColors.White) } };
        public Axis[] FrequencyXAxes { get; } = new[] { new Axis { Name = "Frequency (hz)", NamePaint = new SolidColorPaint(SKColors.White), LabelsPaint = new SolidColorPaint(SKColors.White) } };
        public Axis[] FrequencyYAxes { get; } = new[] { new Axis { Name = "Magnitude", NamePaint = new SolidColorPaint(SKColors.White), LabelsPaint = new SolidColorPaint(SKColors.White) } };
        public Axis[] SineXAxes { get; } = new[] { new Axis { Name = "Time (ms)", NamePaint = new SolidColorPaint(SKColors.White), LabelsPaint = new SolidColorPaint(SKColors.White) } };
        public Axis[] SineYAxes { get; } = new[] { new Axis { Name = "Amplitude", NamePaint = new SolidColorPaint(SKColors.White), LabelsPaint = new SolidColorPaint(SKColors.White) } };

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        #region Handlers

        private void InitializePreloadedAudioMenuItems(object sender, EventArgs e)
        {
            MenuItemUploadAudio.InitializePreloadedAudioMenuItems(MenuItemUploadPreloadedAudio);
        }

        private async void MenuItemUploadAudioClick(object sender, RoutedEventArgs e)
        {
            await MenuItemUploadAudio.UploadAudio();
        }

        private void AudioPlayerClick(object sender, RoutedEventArgs e)
        {
            MenuItemPlayAudio.PlayStopAudio(UploadedAudio, MenuItemAudioPlayer);
        }

        #endregion

        public async Task ChartAudioAsync(string audioFilePath)
        {
            double sineDuration = 0.1;
            double frequencySelectionFactor = 0.5;

            var audioData = await ReadAudioData.ReadAudioAsync(audioFilePath);

            InitializeAmplitudeSeries();
            var amplitudeData = Amplitude.CalculateAmplitude(audioData, SampleRate, true);
            await UpdateChartAmplitudeAsync(amplitudeData);

            InitializeFrequencySeries();
            var fftPoints = await FastFourierTransform.FFTAudioAsync(audioData);
            await UpdateChartFrequencyAsync(fftPoints);

            var threshold = frequencySelectionFactor * fftPoints.Max(x => x.magnitudes);
            var highMagnitudeFrequencies = fftPoints
                .Where(x => x.magnitudes >= threshold)
                .Select(x => x.frequencies)
                .ToArray();

            var sinePoints = highMagnitudeFrequencies
                .Select(frequency => SineWave.GenerateSineGraph(frequency, sineDuration, SampleRate))
                .ToList();

            InitializeSineSeries(highMagnitudeFrequencies);
            await UpdateChartSineAsync(sinePoints);
        }

        #region InitializeCharts

        private void InitializeAmplitudeSeries()
        {
            _amplitudeSeriesCollection = new[]
            {
                new LineSeries<ObservablePoint>
                {
                    Name = "Amplitude",
                    MiniatureShapeSize = 0.1,
                    Stroke = new LinearGradientPaint(new[]{ new SKColor(43, 50, 178), new SKColor(20, 136, 204) }) { StrokeThickness = 1 },
                    Fill = null,
                    GeometryFill = null,
                    GeometryStroke = null,
                    TooltipLabelFormatter = (chartPoint) => $"{chartPoint.Context.Series.Name}: {chartPoint.PrimaryValue}"
                }
            };
            ChartAmplitude.Series = _amplitudeSeriesCollection;
        }

        private void InitializeFrequencySeries()
        {
            _frequencySeriesCollection = new[]
            {
                new ColumnSeries<ObservablePoint>
                {
                    Name = "Frequency",
                    MiniatureShapeSize = 0.1,
                    Stroke = new LinearGradientPaint(new[]{ new SKColor(195, 20, 50), new SKColor(240, 0, 0) }) { StrokeThickness = 1 },
                    Fill = null,
                    TooltipLabelFormatter = (chartPoint) => $"{chartPoint.Context.Series.Name}: {chartPoint.SecondaryValue}"
                }
            };
            ChartFrequency.Series = _frequencySeriesCollection;
        }

        private void InitializeSineSeries(double[] frequencies)
        {
            _sineSeriesCollection = frequencies.Select((x, i) => new LineSeries<ObservablePoint>
            {
                Name = x + "Hz",
                MiniatureShapeSize = 0.1,
                Stroke = new LinearGradientPaint(new[] { new SKColor(252, 74, 26), new SKColor(247, 183, 51) }) { StrokeThickness = 1 },
                Fill = null,
                GeometryFill = null,
                GeometryStroke = null,
                TooltipLabelFormatter = (chartPoint) => $"{chartPoint.Context.Series.Name}"
            }).ToArray();
            ChartSin.Series = _sineSeriesCollection;
        }

        #endregion InitializeCharts

        #region UpdateCharts

        private async Task UpdateChartAmplitudeAsync(List<(double time, double amplitude)> AmplitudePoints)
        {
            int numDataPoints = AmplitudePoints.Count;
            int maxDataPoints = 10000;
            int step = (int)Math.Ceiling((double)numDataPoints / maxDataPoints);

            _amplitudeDataPoints.Clear();
            for (int i = 0; i < numDataPoints; i += step)
            {
                _amplitudeDataPoints.Add(new ObservablePoint(AmplitudePoints[i].time, AmplitudePoints[i].amplitude));
            }
            _amplitudeSeriesCollection[0].Values = _amplitudeDataPoints;

            await Task.Delay(10);
        }

        private async Task UpdateChartFrequencyAsync(List<(double frequencies, double magnitudes)> FFTPoints)
        {
            int numDataPoints = FFTPoints.Count;
            int maxDataPoints = 5000;
            int step = (int)Math.Ceiling((double)numDataPoints / maxDataPoints);

            _frequencyDataPoints.Clear();
            for (int i = 0; i < numDataPoints; i += step)
            {
                _frequencyDataPoints.Add(new ObservablePoint(FFTPoints[i].frequencies, FFTPoints[i].magnitudes));
            }
            _frequencySeriesCollection[0].Values = _frequencyDataPoints;

            await Task.Delay(10);
        }

        private async Task UpdateChartSineAsync(List<List<(double time, double amplitude)>> sinePoints)
        {
            List<List<ObservablePoint>> points = sinePoints.Select(
                    series => series.Select(point => new ObservablePoint(point.time, point.amplitude)).ToList()).ToList();

            for (int i = 0; i < points.Count; i++)
            {
                _sineSeriesCollection[i].Values = points[i];
            }

            await Task.Delay(10);
        }

        #endregion
    }
}