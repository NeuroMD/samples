using System.Drawing;

namespace EmotionalStates.Drawable
{
    public interface IDrawable
    {
        Size DrawableSize { set; }
        void Draw(Graphics graphics);
    }
}
