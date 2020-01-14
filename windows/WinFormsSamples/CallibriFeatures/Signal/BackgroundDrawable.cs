using System;
using System.Collections.Generic;
using System.Drawing;
using CallibriFeatures.GraphicsControl;

namespace CallibriFeatures.Signal
{
    public class BackgroundDrawable : IDrawable
    {
        public SizeF DrawableSize { get; set; }

        public IEnumerable<IBackgroundZone> BackgroundZones { private get; set; }

        public BackgroundDrawable()
        {
            BackgroundZones = new IBackgroundZone[]{new BackgroundZone(0, DrawableSize.Width, Color.AliceBlue)};
        }

        public void Draw(Graphics graphics)
        {
            foreach (var backgroundZone in BackgroundZones)
            {
                using (var zoneBrush = new SolidBrush(backgroundZone.Color))
                {
                    graphics.FillRectangle
                    (
                        zoneBrush, 
                        backgroundZone.Left, 
                        0, 
                        backgroundZone.Width,
                        DrawableSize.Height
                    );
                }
            }

            var quarterY = DrawableSize.Height / 4;
            graphics.DrawLine(Pens.DarkGray, 0, quarterY, 40, quarterY);
            graphics.DrawLine(Pens.DarkGray, 0, 3 * quarterY, 40, 3 * quarterY);
            graphics.DrawLine(Pens.DarkGray, 0, 2*quarterY, DrawableSize.Width, 2*quarterY);
            graphics.DrawLine(Pens.DarkGray, DrawableSize.Width - 40, quarterY, DrawableSize.Width, quarterY);
            graphics.DrawLine(Pens.DarkGray, DrawableSize.Width - 40, 3 * quarterY, DrawableSize.Width, 3 * quarterY);
        }
    }
}