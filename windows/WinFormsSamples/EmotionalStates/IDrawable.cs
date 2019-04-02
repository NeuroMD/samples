using System.Drawing;

namespace EmotionalStates
{
    public interface IDrawable
    {
        Size DrawableSize { set; }
        void Draw(Graphics graphics);
    }
}
