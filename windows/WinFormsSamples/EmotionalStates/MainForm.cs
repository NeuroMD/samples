using System;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using EmotionalStates.Devices;
using EmotionalStates.Drawable;
using EmotionalStates.EmotionsChart;
using EmotionalStates.IndexChart;
using EmotionalStates.Spectrum;

namespace EmotionalStates
{
    public partial class MainForm : Form
    {
        private IndexChartPresenter _indexChartPresenter;
        private EmotionsChartPresenter _emotionsChartPresenter;
        private SpectrumChartPresenter _spectrumChartPresenter;
        private StatesSettingsPresenter _statesSettingsPresenter;
        private IndexSettingsPresenter _indexSettingsPresenter;
        private readonly DeviceModel _deviceModel = new DeviceModel();
        private readonly EmotionsChart.EmotionsChart _statesChart = new EmotionsChart.EmotionsChart {Mode = EmotionBarMode.Empty};

    public MainForm()
        {
            InitializeComponent();
            _deviceModel.ChannelsChanged += _deviceModel_ChannelsChanged;
        }

        private void _deviceModel_ChannelsChanged(object sender, System.EventArgs e)
        {
            if (_deviceModel.IndexChannel != null)
            {
                BeginInvoke((MethodInvoker) delegate
                {
                    _startSignalButton.Enabled = true;
                    _stopSignalButton.Enabled = true;

                    var indexChart = new EegIndexChart();
                    _indexChartPresenter = new IndexChartPresenter(indexChart, _deviceModel.IndexChannel,
                        _emotionsStartButton, _statesStopButton, this);
                    _indexSettingsPresenter = new IndexSettingsPresenter(_indexSettingsControl, _deviceModel.IndexChannel);
                    _indexSettingsControl.Enabled = true;
                    var spectrumChartT3O1 = new SpectrumChart();
                    var spectrumChartT4O2 = new SpectrumChart();
                    _spectrumChartPresenter = new SpectrumChartPresenter(spectrumChartT3O1, _spectrumAmplitudeTrackBar,
                        new SpectrumModel(_deviceModel.T3O1SpectrumChannel));
                    _spectrumChartPresenter = new SpectrumChartPresenter(spectrumChartT4O2, _spectrumAmplitudeTrackBar,
                        new SpectrumModel(_deviceModel.T4O2SpectrumChannel));

                    var compositeDrawable = new CompoundDrawable();
                    compositeDrawable.AddDrawable(indexChart, new PointF(0f, 0f), new SizeF(0.7f, 0.94f));
                    compositeDrawable.AddDrawable(spectrumChartT3O1, new PointF(0.7f, 0.0f), new SizeF(0.3f, 0.47f));
                    compositeDrawable.AddDrawable(spectrumChartT4O2, new PointF(0.7f, 0.47f), new SizeF(0.3f, 0.47f));
                    compositeDrawable.AddDrawable(_statesChart, new PointF(0f, 0.94f), new SizeF(1.0f, 0.06f));
                    _drawableControl.Drawable = compositeDrawable;

                });
            }
            else
            {
                BeginInvoke((MethodInvoker)delegate
                {
                    _startSignalButton.Enabled = false;
                    _stopSignalButton.Enabled = false;
                });
                _drawableControl.Drawable = new EmptyDrawable();
            }
        }

        private void _findDeviceButton_Click(object sender, System.EventArgs e)
        {
            _deviceModel.StopSignal();
            var deviceSearchForm = new DeviceSearchForm();
            if (deviceSearchForm.ShowDialog(this) == DialogResult.OK)
            {
                if (deviceSearchForm.SelectedDevice != null)
                {
                    _deviceLabel.Text = deviceSearchForm.SelectedDevice.ToString();
                    _deviceModel.SelectDevice(deviceSearchForm.SelectedDevice);
                }
                else
                {
                    _deviceLabel.Text = "Not selected";
                }
            }
        }

        private void _startSignalButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                _deviceModel.StartSignal();
                _indexChartPresenter.OnSignalStarted();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }
        }

        private void _stopSignalButton_Click(object sender, System.EventArgs e)
        {
            _deviceModel.StopSignal();
        }

        private void _emotionsStartButton_Click(object sender, System.EventArgs e)
        {
            if (_emotionsChartPresenter != null)
                return;

            try
            {
                var channel = _deviceModel.CreateEmotionChannel();
                _emotionsChartPresenter = new EmotionsChartPresenter(_statesChart, channel);
                _statesSettingsPresenter = new StatesSettingsPresenter(_emotionCoefficientsControl, channel);
                _emotionCoefficientsControl.Enabled = true;
                SetDestination();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }
        }

        private void _statesStopButton_Click(object sender, EventArgs e)
        {
            _emotionCoefficientsControl.Enabled = false;
            _deviceModel.StopSignal();
            _emotionsChartPresenter?.Dispose();
            _emotionsChartPresenter = null;
            _statesChart.Mode = EmotionBarMode.Empty;
            _deviceModel.StartSignal();
        }

        private void _broadcastCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _applyNetSettingsButton.Enabled = true;
            _ipAddressTextBox.Enabled = !_broadcastCheckBox.Checked;
        }

        private void _portTextBox_TextChanged(object sender, EventArgs e)
        {
            _applyNetSettingsButton.Enabled = true;
        }

        private void _ipAddressTextBox_TextChanged(object sender, EventArgs e)
        {
            _applyNetSettingsButton.Enabled = true;
        }

        private void _applyNetSettingsButton_Click(object sender, EventArgs e)
        {
            if (_emotionsChartPresenter == null)
                return;

            SetDestination();
        }

        private void SetDestination()
        {
            try
            {
                var address = _broadcastCheckBox.Checked ? IPAddress.Broadcast : IPAddress.Parse(_ipAddressTextBox.Text);
                var port = Convert.ToInt32(_portTextBox.Text);
                _emotionsChartPresenter.Destination = new IPEndPoint(address, port);
            }
            catch (FormatException)
            {
                MessageBox.Show("Wrong IP address or port format", "Destination");
            }

            _applyNetSettingsButton.Enabled = false;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _deviceModel.StopSignal();
        }
    }
}
