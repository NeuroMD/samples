using System.Windows.Forms;

namespace EmotionalStates.Drawable
{
    public interface IMouseEventsHandler
    {
        bool OnMouseMove(MouseEventArgs mouseEventArgs);
        bool OnMouseClick(MouseEventArgs mouseEventArgs);
        bool OnMouseDown(MouseEventArgs mouseEventArgs);
        bool OnMouseUp(MouseEventArgs mouseEventArgs);
    }

    public abstract class MouseEventsHandler : IMouseEventsHandler
    {
        public virtual bool OnMouseMove(MouseEventArgs mouseEventArgs) => false;
        public virtual bool OnMouseClick(MouseEventArgs mouseEventArgs) => false;
        public virtual bool OnMouseDown(MouseEventArgs mouseEventArgs) => false;
        public virtual bool OnMouseUp(MouseEventArgs mouseEventArgs) => false;
    }
}