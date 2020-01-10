using System;
using System.Drawing;
using CallibriFeatures.DrawableControl;

namespace CallibriFeatures.Spectrum.View
{
    public class VerticalRuler : IVerticalRuler, IDrawable
    {
        public int Scale { get; set; }
        public bool IsSmall { get; set; }
        public SizeF DrawableSize { private get; set; }

        public void Draw(Graphics graphics)
        {
            FillBackground(graphics, DrawableSize);
            DrawRuler(graphics, Scale, DrawableSize, IsSmall);
        }

        private static void DrawRuler(Graphics graphics, int scale, SizeF drawableSize, bool isSmall)
        {
            if (graphics == null)
                throw new ArgumentNullException(nameof(graphics), "Graphics instance cannot be null");

            using (var nameBrush = new SolidBrush(Color.Black))
            using (var rulerFont = new Font("Tahoma", 9, FontStyle.Regular, GraphicsUnit.Pixel))
            using (var rulerPenThick = new Pen(Color.Black, 2))
            using (var rulerPenThin = new Pen(Color.Black, 1))
            {
                var dashWidth = 4;
                if (!isSmall)
                {
                    dashWidth = 6;
                    graphics.DrawLine(rulerPenThick, 0, drawableSize.Height - 1, dashWidth, drawableSize.Height - 1);
                }
                graphics.DrawLine(rulerPenThick, drawableSize.Width, drawableSize.Height - 1, drawableSize.Width - dashWidth, drawableSize.Height - 1);
                // короткие черточки и подписи
                float sc = 0;
                float y = drawableSize.Height;
                for (var ry = 0; ry < 3; ry++)
                {
                    y -= drawableSize.Height / 4f;
                    sc += scale / 4f;


                    var textSize = graphics.MeasureString(sc.ToString(), rulerFont);
                    var textPos = drawableSize.Width - dashWidth - textSize.Width - 1;
                    graphics.DrawLine(rulerPenThin, drawableSize.Width, y, drawableSize.Width - dashWidth, y);
                    if (!isSmall)
                    {
                        textPos = drawableSize.Width / 2 - textSize.Width / 2;
                        graphics.DrawLine(rulerPenThin, 0, y, dashWidth, y);
                    }

                    //if (sc != scaleString)
                    graphics.DrawString(sc.ToString(), rulerFont, nameBrush, textPos,y - textSize.Height / 2);
                }
            }
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
    }
}