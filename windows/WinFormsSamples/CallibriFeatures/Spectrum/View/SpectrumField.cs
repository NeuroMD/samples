using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using CallibriFeatures.GraphicsControl;
using CallibriFeatures.Signal;

namespace CallibriFeatures.Spectrum.View
{
    public class SpectrumField : MouseEventsHandler, ISpectrumField, IDrawable
    {
        private readonly double _maxFrequency;
        private readonly IReadOnlyCollection<ISpectrumChartBand> _rhythms;
        private IVerticalScan _verticalScan;
        private Point _mousePosition = new Point(-1, -1);

        public string ChannelName { private get; set; }
        public SpectrumArray SpectrumData { private get; set; }

        public IVerticalScan VerticalScan
        {
            set => _verticalScan = value ?? throw new ArgumentNullException(nameof(VerticalScan), "Cannot set vertical scan value from null reference");
        }

        public SizeF DrawableSize { private get; set; }

        public SpectrumField(IReadOnlyCollection<ISpectrumChartBand> rhythms, double maxFrequency, IVerticalScan initialScan)
        {
            _rhythms = rhythms;
            _maxFrequency = maxFrequency;
            VerticalScan = initialScan;
            SpectrumData = new SpectrumArray {FrequencyStep = 1.0, Magnitude = new double[(int)Math.Ceiling(_maxFrequency)*2]};
        }

        public void Draw(Graphics graphics)
        {
            FillBackground(graphics, DrawableSize);
            DrawGrid(graphics, _maxFrequency, DrawableSize);
            DrawSpectrum(graphics, SpectrumData, _verticalScan, _rhythms, _maxFrequency, DrawableSize);
            DrawChannelName(graphics, ChannelName, DrawableSize);
        }

        private static void DrawGrid(Graphics graphics, double maxFrequency, SizeF drawableSize)
        {
            var pixelsPerHz = drawableSize.Width / maxFrequency;
            var freq = 5;
            var x = (float)(5 * pixelsPerHz);
            using (var gridPen = new Pen(Color.LightGray, 1) { DashStyle = DashStyle.Dot })
            {
                do
                {
                    graphics.DrawLine(gridPen, x, 0, x, drawableSize.Height);
                    x += (float)(5 * pixelsPerHz);
                    freq += 5;
                } while (freq < maxFrequency);
            }

            using (var vGridPen = new Pen(Color.LightGray, 1) { DashStyle = DashStyle.Dot })
            {
                graphics.DrawLine(vGridPen, 0, drawableSize.Height-1, drawableSize.Width, drawableSize.Height-1);
                // короткие черточки и подписи
                var y = drawableSize.Height;
                for (var ry = 0; ry < 3; ry++)
                {
                    y -= drawableSize.Height / 4f;
                    graphics.DrawLine(vGridPen, 0, y, drawableSize.Width, y);
                }
            }
        }

        private static void DrawSpectrum(Graphics graphics, SpectrumArray spectrumData, IVerticalScan verticalScan, IReadOnlyCollection<ISpectrumChartBand> rhythms, double maxFrequency, SizeF drawableSize)
        {
            if (graphics == null)
                throw new ArgumentNullException(nameof(graphics), "Graphics instance cannot be null");

            if (spectrumData.Magnitude == null)
                throw new ArgumentNullException(nameof(spectrumData.Magnitude), "Magnitude collection cannot be null");

            if (verticalScan == null)
                throw new ArgumentNullException(nameof(verticalScan), "Vertical scan cannot be null.");

            if (rhythms == null)
                throw new ArgumentNullException(nameof(rhythms), "Rhythms collection cannot be null");

            var lowFreq = rhythms.Min(x => x.FreqBegin);
            var lowFreqIndex = Convert.ToInt32(Math.Round(lowFreq / spectrumData.FrequencyStep));
            var index = lowFreqIndex;
            var pixelsPerHz = drawableSize.Width / maxFrequency;
            var samplesCount = maxFrequency / spectrumData.FrequencyStep;
            var pixelsPerSample = drawableSize.Width / samplesCount;
            var pixelsPerNv = (float)drawableSize.Height / verticalScan.MicroVolts / 1000f;
            foreach (var eegRhythm in rhythms)
            {
                if (eegRhythm == null)
                    throw new DataException("Rhythms collection is corrupted. Rhythm cannot be null");

                var len = 3 + Convert.ToInt32(Math.Round((eegRhythm.FreqEnd - eegRhythm.FreqBegin) / spectrumData.FrequencyStep));
                var points = new PointF[len];

                points[0].X = (float)(0 + eegRhythm.FreqBegin * pixelsPerHz);
                points[0].Y = drawableSize.Height;
                for (var p = 1; p < len - 1; p++)
                {
                    points[p].X = (float)(points[0].X + (p - 1) * pixelsPerSample);
                    points[p].Y = drawableSize.Height - (float)(spectrumData.Magnitude[index] * pixelsPerNv);
                    if (points[p].Y < 0)
                        points[p].Y = 0;
                    index++;
                }
                index--;
                points[len - 1].X = (float)(eegRhythm.FreqEnd * pixelsPerHz);
                points[len - 1].Y = drawableSize.Height;

                var path = new GraphicsPath();
                path.AddLines(points);
                using (var rhythmBrush = new SolidBrush(eegRhythm.Color))
                {
                    graphics.FillPath(rhythmBrush, path);
                }
            }
        }

        private static void DrawChannelName(Graphics graphics, string channelName, SizeF drawableSize)
        {
            if (graphics == null)
                throw new ArgumentNullException(nameof(graphics), "Graphics instance cannot be null");

            if (string.IsNullOrEmpty(channelName))
                return;

            using (var nameFont = new Font("Tahoma", 12, FontStyle.Bold, GraphicsUnit.Pixel))
            using (var nameBrush = new SolidBrush(Color.Black))
            {
                var textSize = graphics.MeasureString(channelName, nameFont);
                graphics.DrawString(channelName, nameFont, nameBrush, drawableSize.Width - textSize.Width - 5, textSize.Height - 5);
            }
        }

        private static void FillBackground(Graphics graphics, SizeF drawableSize)
        {
            if (graphics == null)
                throw new ArgumentNullException(nameof(graphics), "Graphics instance cannot be null");

            using (var fieldBackgroundBrush = new SolidBrush(Color.LightYellow))
            {
                graphics.FillRectangle(fieldBackgroundBrush, 0, 0, drawableSize.Width, drawableSize.Height);
            }
        }
    }
}