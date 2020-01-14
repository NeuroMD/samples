using CallibriFeatures.GraphicsControl;
using CallibriFeatures.Signal;
using Neuro;

namespace CallibriFeatures.Devices
{
    public class SignalPlotPresenter : IModuleControlPresenter
    {
        private readonly DrawableControl _signalPlotControl;
        private readonly SignalChartSettingsControl _settingsControl;

        public SignalPlotPresenter(DrawableControl signalPlotControl, SignalChartSettingsControl settingsControl, Device device)
        {
            _signalPlotControl = signalPlotControl;
            _settingsControl = settingsControl;
            FillVerticalScans(_settingsControl, new SignalChannel(device), );
            FillHorizontalScans(_settingsControl);
        }

        private static void FillHorizontalScans(SignalChartSettingsControl settingsControl)
        {
            var horizontalScans = new IHorizontalScan[]
            {
                new HorizontalScan(1), 
                new HorizontalScan(2), 
                new HorizontalScan(3), 
                new HorizontalScan(5), 
                new HorizontalScan(10), 
                new HorizontalScan(20),
            };
            settingsControl.HorizontalScans = horizontalScans;
            settingsControl.SelectedHorizontalScan = horizontalScans[4];
        }

        private static void FillVerticalScans(SignalChartSettingsControl settingsControl, IDataChannel<double> dataChannel, ICalculationSpan viewSpan)
        {
            var verticalScans = new IVerticalScan[]
            {
                new AutoVerticalScan(dataChannel, viewSpan),
                new NumericVerticalScan(1),
                new NumericVerticalScan(2),
                new NumericVerticalScan(5),
                new NumericVerticalScan(10),
                new NumericVerticalScan(20),
                new NumericVerticalScan(50),
                new NumericVerticalScan(100),
                new NumericVerticalScan(200),
                new NumericVerticalScan(500),
                new NumericVerticalScan(1000),
                new NumericVerticalScan(2000),
                new NumericVerticalScan(5000),
                new NumericVerticalScan(10000),
                new NumericVerticalScan(20000),
                new NumericVerticalScan(50000),
                new NumericVerticalScan(100000)
            };
            settingsControl.VerticalScans = verticalScans;
            settingsControl.SelectedVerticalScan = verticalScans[0];
        }

        public bool Enabled
        {
            get => _signalPlotControl.Enabled;
            set => _signalPlotControl.Enabled = value;
        }
    }
}