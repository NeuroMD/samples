using System.Drawing;

namespace CallibriFeatures.GraphicsControl
{
    public interface IDrawable
    {
        SizeF DrawableSize { set; }
        void Draw(Graphics graphics);
    }
}
