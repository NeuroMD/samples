using System;
using System.Drawing;

namespace CallibriFeatures.DrawableControl
{

    internal sealed class EmptyDrawable : IDrawable
    {
        private readonly Color _color;

        public EmptyDrawable(Color color)
        {
            _color = color;
        }

        public EmptyDrawable()
        {
            _color = Color.Black;
        }

        public SizeF DrawableSize
        {
            set
            {
                //do nothing
            }
        }

        public void Draw(Graphics graphics)
        {
            if (graphics == null)
                throw new ArgumentNullException(nameof(graphics), "Graphics cannot be null");

            graphics.Clear(_color);
        }
    }
}