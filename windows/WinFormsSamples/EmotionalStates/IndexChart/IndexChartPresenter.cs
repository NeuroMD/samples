using System;
using System.Windows.Forms;
using Neuro;

namespace EmotionalStates.IndexChart
{
    internal class IndexChartPresenter
    {
        private readonly IEegIndexChart _indexChart;
        private readonly EegIndexChannel _channel;
        private readonly Button _emotionsButton;
        private readonly Button _emotionsStopButton;
        private readonly Control _context;
        private DateTime _startTime;

        public IndexChartPresenter(IEegIndexChart chart, EegIndexChannel channel, Button emotionsButton, Button emotionsStopButton, Control context)
        {
            _indexChart = chart;
            _channel = channel;
            _context = context;
            _emotionsButton = emotionsButton;
            _emotionsStopButton = emotionsStopButton;
            _emotionsButton.Enabled = false;
            _emotionsStopButton.Enabled = false;
            _indexChart.Mode = EegIndexChartMode.Empty;

            _indexChart.SizeChanged += _indexChart_SizeChanged;
            _channel.LengthChanged += _channel_LengthChanged;
        }

        public void OnSignalStarted()
        {
            _indexChart.Mode = EegIndexChartMode.Waiting;
            _startTime = DateTime.Now;
        }

        private void _indexChart_SizeChanged(object sender, System.Drawing.Size e)
        {
            UpdateChartData();
        }

        private void _channel_LengthChanged(object sender, int length)
        {
            if (_indexChart.Mode != EegIndexChartMode.Signal)
            {
                _context.BeginInvoke((MethodInvoker) delegate
                { 
                    _emotionsButton.Enabled = true;
                    _emotionsStopButton.Enabled = true;
                });
                _indexChart.Mode = EegIndexChartMode.Signal;
            }

            UpdateChartData();
        }

        private void UpdateChartData()
        {
            var readLength = _channel.TotalLength < _indexChart.DrawableSize.Width
                ? _channel.TotalLength
                : _indexChart.DrawableSize.Width;

            if (readLength <= 0) return;

            var offset = _channel.TotalLength - readLength;
            var data = _channel.ReadData(offset, readLength);
            _indexChart.IndicesData = data;
            if (_channel.Mode == EegIndexMode.LeftSide)
            {
                _indexChart.ChannelName = "T3O1";
            }
            else if (_channel.Mode == EegIndexMode.RightSide)
            {
                _indexChart.ChannelName = "T4O2";
            }
            else
            {
                _indexChart.ChannelName = "Artifacts";
            }
            _indexChart.LastIndexTime = _channel.TotalLength; //(int)(DateTime.Now - _startTime).TotalMilliseconds; //_channel.TotalLength;
        }
    }
}