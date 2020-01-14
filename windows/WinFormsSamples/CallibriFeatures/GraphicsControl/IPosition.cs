using System.Drawing;

namespace CallibriFeatures.GraphicsControl
{
    public interface IReadonlyPosition
    {
        float X { get; }
        float Y { get; }

        PointF ConvertPosition(SizeF canvasSize);
    }

    public class AbsolutePosition : IReadonlyPosition
    {
        public AbsolutePosition(PointF point)
        {
            X = point.X;
            Y = point.Y;
        }

        public AbsolutePosition(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X { get; }
        public float Y { get; }

        public PointF ConvertPosition(SizeF canvasSize)
        {
            return new PointF(X, Y);
        }
    }

    public class RelativePosition : IReadonlyPosition
    {
        public RelativePosition(PointF point)
        {
            X = point.X;
            Y = point.Y;
        }

        public RelativePosition(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X { get; }
        public float Y { get; }

        public PointF ConvertPosition(SizeF canvasSize)
        {
            return new PointF(canvasSize.Width * X, canvasSize.Height * Y);
        }
    }
}