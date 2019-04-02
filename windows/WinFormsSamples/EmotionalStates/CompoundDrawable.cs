using System.Collections.Generic;
using System.Drawing;

namespace EmotionalStates
{
    public class CompoundDrawable:IDrawable
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
            _drawableAreas.Add(new MappedDrawable(drawable, positionRelative, sizeRelative));
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

        private class MappedDrawable : IDrawable
        {
            private readonly IDrawable _sourceDrawable;
            private readonly PointF _relativePosition;
            private readonly SizeF _relativeSize;

            private Size _currentDrawableSize;

            public MappedDrawable(IDrawable sourceDrawable, PointF positionRelative, SizeF sizeRelative)
            {
                _sourceDrawable = sourceDrawable;
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
                    _sourceDrawable.DrawableSize = new Size((int)width, (int)height);
                }
            }

            public void Draw(Graphics graphics)
            {
                var dx = _currentDrawableSize.Width * _relativePosition.X;
                var dy = _currentDrawableSize.Height * _relativePosition.Y;
                graphics.TranslateTransform(dx, dy);
                _sourceDrawable.Draw(graphics);
                graphics.TranslateTransform(-dx, -dy);
            }
        }
    }
}