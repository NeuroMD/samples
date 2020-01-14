using Neuro;

namespace CallibriFeatures.Signal
{
    public class SignalDrawablePresenter : IPlotDrawablePresenter
    {
        private readonly SignalDrawable _drawable;
        private readonly IDataChannel<double> _dataChannel;

        public SignalDrawablePresenter(SignalDrawable drawable, IDataChannel<double> dataChannel)
        {
            _drawable = drawable;
            _dataChannel = dataChannel;
        }

        public ICalculationSpan ViewSpan { get; set; }
    }
}