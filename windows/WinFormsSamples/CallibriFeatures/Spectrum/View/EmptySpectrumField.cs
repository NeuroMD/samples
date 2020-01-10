using System;
using System.Drawing;
using CallibriFeatures.DrawableControl;

namespace CallibriFeatures.Spectrum.View
{
    public class EmptySpectrumField : IDrawable
    {
        public SizeF DrawableSize { get; set; }

        public void Draw(Graphics graphics)
        {
            if (graphics == null)
                throw new ArgumentNullException(nameof(graphics), "Graphics instance cannot be null");

            using (var fieldBackgroundBrush = new SolidBrush(Color.LightYellow))
            {
                graphics.FillRectangle(fieldBackgroundBrush, 0, 0, DrawableSize.Width, DrawableSize.Height);
            }
        }
    }
}