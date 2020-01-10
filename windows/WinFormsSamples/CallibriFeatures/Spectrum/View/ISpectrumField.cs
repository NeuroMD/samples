using System.Collections.Generic;
using CallibriFeatures.Signal;

namespace CallibriFeatures.Spectrum.View
{
    public struct SpectrumArray
    {
        public IReadOnlyList<double> Magnitude;
        public double FrequencyStep;
    }
    public interface ISpectrumField
    {
        string ChannelName { set; }
        SpectrumArray SpectrumData { set; }
        IVerticalScan VerticalScan { set; }
    }
}