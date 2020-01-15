using System;

namespace CallibriFeatures.Signal
{
    public interface ICalculationSpan
    {
        double StartTime { get; }
        double Duration { get; }
        event EventHandler SpanChanged;
    }
}