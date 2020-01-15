using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CallibriFeatures.GraphicsControl
{
    public class CompoundDrawable : IDrawable, IMouseEventsHandler
    {
        private readonly IList<MappedDrawable> _drawableAreas = new List<MappedDrawable>();

        public SizeF DrawableSize
        {
            set
            {
                if (_drawableAreas == null)
                    throw new InvalidOperationException("Unexpected drawable state: drawable areas container is null");

                foreach (var mappedDrawable in _drawableAreas)
                {
                    mappedDrawable.DrawableSize = value;
                }
            }
        }

        public void AddDrawable(IDrawable drawable, IReadonlyPosition position, IReadonlySize size)
        {
            var newDrawable = drawable is IMouseEventsHandler
                ? new MouseEventsDrawable(drawable, position, size)
                : new MappedDrawable(drawable, position, size);

            if (_drawableAreas == null)
                throw new InvalidOperationException("Unexpected drawable state: drawable areas container is null");

            _drawableAreas.Add(newDrawable);
        }

        public void Draw(Graphics graphics)
        {
            if (_drawableAreas == null)
                throw new InvalidOperationException("Unexpected drawable state: drawable areas container is null");

            foreach (var mappedDrawable in _drawableAreas)
            {
                mappedDrawable.Draw(graphics);
            }
        }

        public void ClearDrawableAreas()
        {
            if (_drawableAreas == null)
                throw new InvalidOperationException("Unexpected drawable state: drawable areas container is null");

            _drawableAreas.Clear();
        }
        
        public bool OnMouseMove(MouseEventArgs mouseEventArgs)
        {
            if (_drawableAreas == null)
                throw new InvalidOperationException("Unexpected drawable state: drawable areas container is null");

            var drawableLeft = _drawableAreas.OfType<IMouseEventsHandler>().Where(drawable => drawable.OnMouseMove(mouseEventArgs) == false);
            foreach (var handler in drawableLeft)
            {
                handler.OnMouseLeave();
            }
            return drawableLeft.Count() != _drawableAreas.Count;
        }

        public bool OnMouseClick(MouseEventArgs mouseEventArgs)
        {
            if (_drawableAreas == null)
                throw new InvalidOperationException("Unexpected drawable state: drawable areas container is null");

            return _drawableAreas.OfType<IMouseEventsHandler>().Any(drawable => drawable.OnMouseClick(mouseEventArgs));
        }

        public bool OnMouseDown(MouseEventArgs mouseEventArgs)
        {
            if (_drawableAreas == null)
                throw new InvalidOperationException("Unexpected drawable state: drawable areas container is null");

            return _drawableAreas.OfType<IMouseEventsHandler>().Any(drawable => drawable.OnMouseDown(mouseEventArgs));
        }

        public bool OnMouseUp(MouseEventArgs mouseEventArgs)
        {
            if (_drawableAreas == null)
                throw new InvalidOperationException("Unexpected drawable state: drawable areas container is null");

            return _drawableAreas.OfType<IMouseEventsHandler>().Any(drawable => drawable.OnMouseUp(mouseEventArgs));
        }

        public bool OnMouseLeave()
        {
            if (_drawableAreas == null)
                throw new InvalidOperationException("Unexpected drawable state: drawable areas container is null");

            return _drawableAreas.OfType<IMouseEventsHandler>().Any(drawable => drawable.OnMouseLeave());
        }

        private class MappedDrawable : IDrawable
        {
            protected readonly IDrawable SourceDrawable;
            protected readonly IReadonlyPosition Position;
            protected readonly IReadonlySize Size;

            protected SizeF CurrentDrawableSize;

            public MappedDrawable(IDrawable sourceDrawable, IReadonlyPosition position, IReadonlySize size)
            {
                SourceDrawable = sourceDrawable;
                Position = position;
                Size = size;
            }

            public SizeF DrawableSize
            {
                set
                {
                    if (SourceDrawable == null)
                        throw new InvalidOperationException("Unexpected drawable state: SourceDrawable is null");

                    CurrentDrawableSize = value;
                    SourceDrawable.DrawableSize = Size.ConvertSize(CurrentDrawableSize);
                }
            }

            public void Draw(Graphics graphics)
            {
                if (SourceDrawable == null)
                    throw new InvalidOperationException("Unexpected drawable state: SourceDrawable is null");

                if (graphics == null)
                    throw new ArgumentNullException(nameof(graphics), "Graphics cannot be null");

                var realPosition = Position.ConvertPosition(CurrentDrawableSize);
                graphics.TranslateTransform(realPosition.X, realPosition.Y);
                SourceDrawable.Draw(graphics);
                graphics.TranslateTransform(-realPosition.X, -realPosition.Y);
            }
        }

        private class MouseEventsDrawable : MappedDrawable, IMouseEventsHandler
        {
            public MouseEventsDrawable(IDrawable sourceDrawable, IReadonlyPosition position, IReadonlySize size) : base(
                sourceDrawable, position, size)
            {
                if (!(sourceDrawable is IMouseEventsHandler))
                {
                    throw new ArgumentException(
                        "Unable to create mapped drawable with mouse event handling from object without IMouseEventHandler interface");
                }
            }

            public bool OnMouseMove(MouseEventArgs mouseEventArgs)
            {
                if (!(SourceDrawable is IMouseEventsHandler))
                    return false;

                if (mouseEventArgs == null)
                    throw new ArgumentNullException(nameof(mouseEventArgs), "MouseEventArgs cannot be null");

                var realPosition = Position.ConvertPosition(CurrentDrawableSize);
                if (!MouseIsOnDrawable(mouseEventArgs)) return false;

                return ((IMouseEventsHandler) SourceDrawable).OnMouseMove(
                    new MouseEventArgs(
                        mouseEventArgs.Button, 
                        mouseEventArgs.Clicks, 
                        (int)(mouseEventArgs.X - realPosition.X), 
                        (int)(mouseEventArgs.Y - realPosition.Y), 
                        mouseEventArgs.Delta)
                    );
            }

            public bool OnMouseClick(MouseEventArgs mouseEventArgs)
            {
                if (!(SourceDrawable is IMouseEventsHandler))
                    return false;

                if (mouseEventArgs == null)
                    throw new ArgumentNullException(nameof(mouseEventArgs), "MouseEventArgs cannot be null");

                var realPosition = Position.ConvertPosition(CurrentDrawableSize);
                if (!MouseIsOnDrawable(mouseEventArgs)) return false;

                return ((IMouseEventsHandler)SourceDrawable).OnMouseClick(
                    new MouseEventArgs(
                        mouseEventArgs.Button,
                        mouseEventArgs.Clicks,
                        (int)(mouseEventArgs.X - realPosition.X),
                        (int)(mouseEventArgs.Y - realPosition.Y),
                        mouseEventArgs.Delta)
                );
            }

            public bool OnMouseDown(MouseEventArgs mouseEventArgs)
            {
                if (!(SourceDrawable is IMouseEventsHandler))
                    return false;

                if (mouseEventArgs == null)
                    throw new ArgumentNullException(nameof(mouseEventArgs), "MouseEventArgs cannot be null");

                var realPosition = Position.ConvertPosition(CurrentDrawableSize);
                if (!MouseIsOnDrawable(mouseEventArgs)) return false;

                return ((IMouseEventsHandler)SourceDrawable).OnMouseDown(
                    new MouseEventArgs(
                        mouseEventArgs.Button,
                        mouseEventArgs.Clicks,
                        (int)(mouseEventArgs.X - realPosition.X),
                        (int)(mouseEventArgs.Y - realPosition.Y),
                        mouseEventArgs.Delta)
                );
            }

            public bool OnMouseUp(MouseEventArgs mouseEventArgs)
            {
                if (!(SourceDrawable is IMouseEventsHandler))
                    return false;

                if (mouseEventArgs == null)
                    throw new ArgumentNullException(nameof(mouseEventArgs), "MouseEventArgs cannot be null");

                var realPosition = Position.ConvertPosition(CurrentDrawableSize);
                if (!MouseIsOnDrawable(mouseEventArgs)) return false;

                return ((IMouseEventsHandler)SourceDrawable).OnMouseUp(
                    new MouseEventArgs(
                        mouseEventArgs.Button,
                        mouseEventArgs.Clicks,
                        (int)(mouseEventArgs.X - realPosition.X),
                        (int)(mouseEventArgs.Y - realPosition.Y),
                        mouseEventArgs.Delta)
                );
            }

            public bool OnMouseLeave()
            {
                return SourceDrawable is IMouseEventsHandler handler && handler.OnMouseLeave();
            }

            private bool MouseIsOnDrawable(MouseEventArgs mouseEventArgs)
            {
                if (mouseEventArgs == null)
                    throw new ArgumentNullException(nameof(mouseEventArgs), "MouseEventArgs cannot be null");

                var realPosition = Position.ConvertPosition(CurrentDrawableSize);
                var realSize = Size.ConvertSize(CurrentDrawableSize);

                return mouseEventArgs.X >= realPosition.X && mouseEventArgs.X <= realPosition.X + realSize.Width &&
                       mouseEventArgs.Y >= realPosition.Y && mouseEventArgs.Y <= realPosition.Y + realSize.Height;
            }
        }
    }
}