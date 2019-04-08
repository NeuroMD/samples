using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EmotionalStates.Drawable
{
    public class CompoundDrawable : IDrawable, IMouseEventsHandler
    {
        private readonly IList<MappedDrawable> _drawableAreas = new List<MappedDrawable>();

        public Size DrawableSize
        {
            set
            {
                foreach (var mappedDrawable in _drawableAreas)
                {
                    mappedDrawable.DrawableSize = value;
                }
            }
        }

        public void AddDrawable(IDrawable drawable, PointF positionRelative, SizeF sizeRelative)
        {
            var newDrawable = drawable is IMouseEventsHandler
                ? new MouseEventsDrawable(drawable, positionRelative, sizeRelative)
                : new MappedDrawable(drawable, positionRelative, sizeRelative);
            _drawableAreas.Add(newDrawable);
        }

        public void Draw(Graphics graphics)
        {
            foreach (var mappedDrawable in _drawableAreas)
            {
                mappedDrawable.Draw(graphics);
            }
        }

        public void ClearDrawableAreas()
        {
            _drawableAreas.Clear();
        }
        
        public bool OnMouseMove(MouseEventArgs mouseEventArgs)
        {
            return _drawableAreas.OfType<IMouseEventsHandler>().Any(drawable => drawable.OnMouseMove(mouseEventArgs));
        }

        public bool OnMouseClick(MouseEventArgs mouseEventArgs)
        {
            return _drawableAreas.OfType<IMouseEventsHandler>().Any(drawable => drawable.OnMouseClick(mouseEventArgs));
        }

        public bool OnMouseDown(MouseEventArgs mouseEventArgs)
        {
            return _drawableAreas.OfType<IMouseEventsHandler>().Any(drawable => drawable.OnMouseDown(mouseEventArgs));
        }

        public bool OnMouseUp(MouseEventArgs mouseEventArgs)
        {
            return _drawableAreas.OfType<IMouseEventsHandler>().Any(drawable => drawable.OnMouseUp(mouseEventArgs));
        }

        private class MappedDrawable : IDrawable
        {
            protected readonly IDrawable SourceDrawable;
            protected readonly PointF _relativePosition;
            protected readonly SizeF _relativeSize;

            protected Size _currentDrawableSize;

            public MappedDrawable(IDrawable sourceDrawable, PointF positionRelative, SizeF sizeRelative)
            {
                SourceDrawable = sourceDrawable;
                _relativePosition = positionRelative;
                _relativeSize = sizeRelative;
            }

            public Size DrawableSize
            {
                set
                {
                    _currentDrawableSize = value;
                    var width = _currentDrawableSize.Width * _relativeSize.Width;
                    var height = _currentDrawableSize.Height * _relativeSize.Height;
                    SourceDrawable.DrawableSize = new Size((int)width, (int)height);
                }
            }

            public void Draw(Graphics graphics)
            {
                var dx = _currentDrawableSize.Width * _relativePosition.X;
                var dy = _currentDrawableSize.Height * _relativePosition.Y;
                graphics.TranslateTransform(dx, dy);
                SourceDrawable.Draw(graphics);
                graphics.TranslateTransform(-dx, -dy);
            }
        }

        private class MouseEventsDrawable : MappedDrawable, IMouseEventsHandler
        {
            public MouseEventsDrawable(IDrawable sourceDrawable, PointF positionRelative, SizeF sizeRelative) : base(
                sourceDrawable, positionRelative, sizeRelative)
            {
                if (!(sourceDrawable is IMouseEventsHandler))
                {
                    throw new ArgumentException(
                        "Unable to create mapped drawable with mouse event handling from object without IMouseEventHandler interface");
                }
            }

            public bool OnMouseMove(MouseEventArgs mouseEventArgs)
            {
                var dx = _currentDrawableSize.Width * _relativePosition.X;
                var dy = _currentDrawableSize.Height * _relativePosition.Y;
                if (!MouseIsOnDrawable(mouseEventArgs)) return false;

                return ((IMouseEventsHandler) SourceDrawable).OnMouseMove(
                    new MouseEventArgs(
                        mouseEventArgs.Button, 
                        mouseEventArgs.Clicks, 
                        (int)(mouseEventArgs.X - dx), 
                        (int)(mouseEventArgs.Y - dy), 
                        mouseEventArgs.Delta)
                    );
            }

            public bool OnMouseClick(MouseEventArgs mouseEventArgs)
            {
                var dx = _currentDrawableSize.Width * _relativePosition.X;
                var dy = _currentDrawableSize.Height * _relativePosition.Y;
                if (!MouseIsOnDrawable(mouseEventArgs)) return false;

                return ((IMouseEventsHandler)SourceDrawable).OnMouseClick(
                    new MouseEventArgs(
                        mouseEventArgs.Button,
                        mouseEventArgs.Clicks,
                        (int)(mouseEventArgs.X - dx),
                        (int)(mouseEventArgs.Y - dy),
                        mouseEventArgs.Delta)
                );
            }

            public bool OnMouseDown(MouseEventArgs mouseEventArgs)
            {
                var dx = _currentDrawableSize.Width * _relativePosition.X;
                var dy = _currentDrawableSize.Height * _relativePosition.Y;
                if (!MouseIsOnDrawable(mouseEventArgs)) return false;

                return ((IMouseEventsHandler)SourceDrawable).OnMouseDown(
                    new MouseEventArgs(
                        mouseEventArgs.Button,
                        mouseEventArgs.Clicks,
                        (int)(mouseEventArgs.X - dx),
                        (int)(mouseEventArgs.Y - dy),
                        mouseEventArgs.Delta)
                );
            }

            public bool OnMouseUp(MouseEventArgs mouseEventArgs)
            {
                var dx = _currentDrawableSize.Width * _relativePosition.X;
                var dy = _currentDrawableSize.Height * _relativePosition.Y;
                if (!MouseIsOnDrawable(mouseEventArgs)) return false;

                return ((IMouseEventsHandler)SourceDrawable).OnMouseUp(
                    new MouseEventArgs(
                        mouseEventArgs.Button,
                        mouseEventArgs.Clicks,
                        (int)(mouseEventArgs.X - dx),
                        (int)(mouseEventArgs.Y - dy),
                        mouseEventArgs.Delta)
                );
            }

            private bool MouseIsOnDrawable(MouseEventArgs mouseEventArgs)
            {
                var dx = _currentDrawableSize.Width * _relativePosition.X;
                var dy = _currentDrawableSize.Height * _relativePosition.Y;
                var width = _currentDrawableSize.Width * _relativeSize.Width;
                var height = _currentDrawableSize.Height * _relativeSize.Height;

                return mouseEventArgs.X >= dx && mouseEventArgs.X <= dx + width && 
                       mouseEventArgs.Y >= dy && mouseEventArgs.Y <= dy + height;
            }
        }
    }
}