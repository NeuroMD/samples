using System.Drawing;

namespace CallibriFeatures.Signal
{
    public class BackgroundZone : IBackgroundZone
    {
        public BackgroundZone(float left, float width, Color color, string name = "")
        {
            Left = left;
            Width = width;
            Color = color;
            Name = name;
        }

        public string Name { get; }
        public float Left { get; }
        public float Width { get; }
        public Color Color { get; }
    }
}