using System;
using System.Linq;
using Neuro;

namespace CallibriFeatures.Signal
{
    public class NumericVerticalScan : IVerticalScan
    {
        public NumericVerticalScan(int value)
        {
            MicroVolts = value;
        }

        public int MicroVolts { get; }

        public override string ToString()
        {
            return $"{MicroVolts} uV";
        }
    }

    public class AutoVerticalScan : IVerticalScan
    {
        private readonly IDataChannel<double> _dataChannel;
        private readonly ICalculationSpan _viewSpan;

        public AutoVerticalScan(IDataChannel<double> dataChannel, ICalculationSpan viewSpan)
        {
            _dataChannel = dataChannel;
            _viewSpan = viewSpan;
            _viewSpan.SpanChanged += _viewSpan_SpanChanged;
        }

        private void _viewSpan_SpanChanged(object sender, System.EventArgs e)
        {
            var offset = (int)(_viewSpan.StartTime * _dataChannel.SamplingFrequency);
            var length = (int)(_viewSpan.Duration * _dataChannel.SamplingFrequency);
            MicroVolts = (int)Math.Ceiling(_dataChannel.ReadData(offset, length).Max());
        }

        public int MicroVolts { get; private set; }

        public override string ToString()
        {
            return "Auto";
        }
    }
}