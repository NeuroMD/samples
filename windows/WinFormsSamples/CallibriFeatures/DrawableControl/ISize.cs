using System.Drawing;

namespace CallibriFeatures.DrawableControl
{
    public interface IReadonlySize
    {
        float Width { get; }
        float Height { get; }

        SizeF ConvertSize(SizeF canvasSize);
    }

    public class AbsoluteSize : IReadonlySize
    {
        public AbsoluteSize(SizeF size)
        {
            Width = size.Width;
            Height = size.Height;
        }

        public AbsoluteSize(float width, float height)
        {
            Width = width;
            Height = height;
        }

        public float Width { get; }
        public float Height { get; }

        public SizeF ConvertSize(SizeF canvasSize)
        {
            return new SizeF(Width, Height);
        }
    }

    public class RelativeSize : IReadonlySize
    {
        public RelativeSize(SizeF size)
        {
            Width = size.Width;
            Height = size.Height;
        }

        public RelativeSize(float width, float height)
        {
            Width = width;
            Height = height;
        }

        public float Width { get; }
        public float Height { get; }

        public SizeF ConvertSize(SizeF canvasSize)
        {
            return new SizeF(Width * canvasSize.Width, canvasSize.Height);
        }
    }
}