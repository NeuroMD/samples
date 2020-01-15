using System.Collections.Generic;
using System.Drawing;
using CallibriFeatures.GraphicsControl;

namespace CallibriFeatures.Signal
{
    public class SignalDrawable : IDrawable
    {
        public SizeF DrawableSize { get; set; }

        public PointF[] SignalPoints { set; private get; }

        public void Draw(Graphics graphics)
        {
            graphics.DrawLines(Pens.DarkBlue, SignalPoints);
        }
    }
}