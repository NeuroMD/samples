using System;
using System.Collections.Generic;
using System.Drawing;
using CallibriFeatures.GraphicsControl;

namespace CallibriFeatures.Spectrum.View
{
    public class HorizontalRuler : IDrawable
    {
        private readonly double _maxFrequency;

        private readonly IReadOnlyCollection<ISpectrumChartBand> _rhythms;

        public SizeF DrawableSize { private get; set; }

        public HorizontalRuler(IReadOnlyCollection<ISpectrumChartBand> rhythms, double maxFrequency)
        {
            _rhythms = rhythms;
            _maxFrequency = maxFrequency;
        }

        public void Draw(Graphics graphics)
        {
            if (graphics == null)
                throw new ArgumentNullException(nameof(graphics), "Graphics instance cannot be null");

            FillBackground(graphics, DrawableSize);
            DrawRuler(graphics, DrawableSize, _maxFrequency);
            DrawRhythms(graphics, _rhythms, DrawableSize, _maxFrequency);
        }

        private static void FillBackground(Graphics graphics, SizeF drawableSize)
        {
            if (graphics == null)
                throw new ArgumentNullException(nameof(graphics), "Graphics instance cannot be null");

            using (var rulerBackgroundBrush = new SolidBrush(SystemColors.Control))
            {
                graphics.FillRectangle(rulerBackgroundBrush, 0, 0, drawableSize.Width, drawableSize.Height);
            }
        }

        private static void DrawRuler(Graphics graphics, SizeF drawableSize, double maxFrequency)
        {
            if (graphics == null)
                throw new ArgumentNullException(nameof(graphics), "Graphics instance cannot be null");

            var pixelsPerHz = drawableSize.Width / maxFrequency;
            using (var rulerPenThin = new Pen(Color.Black, 1))
            {
                var freq = 0;
                var x = 0f;
                graphics.DrawLine(rulerPenThin, 0, 0, drawableSize.Width, 0);
                using (var nameBrush = new SolidBrush(Color.Black))
                using (var rulerPenThick = new Pen(Color.Black, 2))
                using (var gridPen = new Pen(Color.DimGray, 1))
                using (var rulerFont = new Font("Tahoma", 9, FontStyle.Regular, GraphicsUnit.Pixel))
                {
                    // длинные линии и надписи рядом с ними
                    
                    do
                    {
                        graphics.DrawLine(rulerPenThick, x, 0, x, 8);
                        if (freq != 0)
                            graphics.DrawLine(gridPen, x, 0, x, 0);
                        var textSize = graphics.MeasureString(freq.ToString(), rulerFont);
                        var posX = x + 1 - textSize.Width / 2;
                        if (posX < 0) posX = 0;
                        graphics.DrawString(freq.ToString(), rulerFont, nameBrush, posX, 8);
                        x += (float) (5 * pixelsPerHz);
                        freq += 5;
                    } while (freq < maxFrequency);
                }

                // короткие линии
                freq = 0;
                x = 0;
                do
                {
                    graphics.DrawLine(rulerPenThin, x, 0, x, 4);
                    freq++;
                    x += (float) (pixelsPerHz);
                } while (freq < maxFrequency);
            }
        }

        private static void DrawRhythms(Graphics graphics, IEnumerable<ISpectrumChartBand> rhythms, SizeF drawableSize, double maxFrequency)
        {
            if (graphics == null)
                throw new ArgumentNullException(nameof(graphics), "Graphics instance cannot be null");

            if (rhythms == null)
                throw new ArgumentNullException(nameof(rhythms), "Rhythm collection cannot be null");

            var pixelsPerHz = drawableSize.Width / maxFrequency;
            var x = 0f;
            var top = drawableSize.Height / 2f;
            var height = drawableSize.Height - top;
            foreach (var eegRhythm in rhythms)
            {
                if (eegRhythm == null)
                    continue;
                
                x = (float)(eegRhythm.FreqBegin * pixelsPerHz);
                var width = (float) ((eegRhythm.FreqEnd - eegRhythm.FreqBegin) * pixelsPerHz);
                DrawSingleRhythm(graphics, eegRhythm, x, top, width, height);
            }
        }

        private static void DrawSingleRhythm(Graphics graphics, ISpectrumChartBand eegRhythm, float left, float top, float width, float height)
        {
            if (graphics == null)
                throw new ArgumentNullException(nameof(graphics), "Graphics instance cannot be null");

            if (eegRhythm == null)
                throw new ArgumentNullException(nameof(eegRhythm), "Rhythm cannot be null");

            using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            using (var nameBrush = new SolidBrush(Color.Black))
            using (var nameFont = new Font("Tahoma", 12, FontStyle.Bold, GraphicsUnit.Pixel))
            using (var rhythmBrush = new SolidBrush(eegRhythm.Color))
            {
                var rect = new RectangleF { Y = top, Height = height, X = left, Width = width };
                graphics.FillRectangle(rhythmBrush, rect);
                graphics.DrawString(eegRhythm.Name, nameFont, nameBrush, rect, sf);
            }
        }
    }
}