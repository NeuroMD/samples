using System.Drawing;

namespace CallibriFeatures.Signal
{
    public interface IBackgroundZone
    {
        string Name { get; }
        float Left { get; }
        float Width { get; }
        Color Color { get; }
    }
}