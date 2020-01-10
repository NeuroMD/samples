using System.Drawing;

namespace CallibriFeatures.DrawableControl
{
    public interface IDrawable
    {
        SizeF DrawableSize { set; }
        void Draw(Graphics graphics);
    }
}
