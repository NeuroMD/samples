using System.Drawing;

namespace CallibriFeatures.Spectrum
{
    public interface IFrequencyBand
    {
        double FreqBegin { get; }
        double FreqEnd { get; }
    }

    public interface ISpectrumChartBand : IFrequencyBand
    {
        Color Color { get; }
        string Name { get; }
    }
}
