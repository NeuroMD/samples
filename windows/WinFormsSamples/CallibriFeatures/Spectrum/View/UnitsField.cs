using System;
using System.Drawing;
using CallibriFeatures.GraphicsControl;

namespace CallibriFeatures.Spectrum.View
{
    public class UnitsField : IDrawable
    {
        public SizeF DrawableSize { get; set; }

        public void Draw(Graphics graphics)
        {
            FillBackground(graphics, DrawableSize);
            DrawUnits(graphics, DrawableSize);
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

        private static void DrawUnits(Graphics graphics, SizeF drawableSize)
        {
            using (var dimensionFont = new Font("Tahoma", 10, FontStyle.Bold, GraphicsUnit.Pixel))
            using (var nameBrush = new SolidBrush(Color.Black))
            using (var verticalStringFormat = new StringFormat(StringFormatFlags.DirectionVertical | StringFormatFlags.DirectionRightToLeft))
            {
                const string unitsHz = "Hz";
                var textSize = graphics.MeasureString(unitsHz, dimensionFont);
                graphics.DrawString
                (
                    unitsHz, 
                    dimensionFont, 
                    nameBrush, 
                    drawableSize.Width/2 - textSize.Width/2, 
                    drawableSize.Height - textSize.Height - 3
                );

                const string unitsUv = "uV";
                textSize = graphics.MeasureString(unitsUv, dimensionFont);
                graphics.DrawString
                (
                    unitsUv, 
                    dimensionFont, 
                    nameBrush, 
                    drawableSize.Width / 2 + textSize.Height / 2,
                    3,
                    verticalStringFormat
                );
            }
        }
    }
}