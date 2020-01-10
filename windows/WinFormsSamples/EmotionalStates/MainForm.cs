using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using EmotionalStates.Devices;
using EmotionalStates.Drawable;
using EmotionalStates.EmotionsChart;
using EmotionalStates.IndexChart;
using EmotionalStates.SignalChart;
using EmotionalStates.Spectrum;
using Neuro;

namespace EmotionalStates
{
    public partial class MainForm : Form
    {
        private bool _signalIsActive = false;
        private IndexChartPresenter _indexChartPresenter;
        private EmotionsChartPresenter _emotionsChartPresenter;
        private SpectrumChartPresenter _spectrumChartPresenter;
        private readonly DeviceModel _deviceModel = new DeviceModel();
        private readonly EmotionsChart.EmotionsChart _statesChart = new EmotionsChart.EmotionsChart { Mode = EmotionBarMode.Empty };
        //        private readonly SignalViewController[] _signalViewController = new SignalViewController[2];

        public MainForm()
        {
            InitializeComponent();
            _deviceModel.ChannelsChanged += _deviceModel_ChannelsChanged;

            //            _signalViewController[0] = new SignalViewController(this, signalChart1);
            //            _signalViewController[1] = new SignalViewController(this, signalChart2);


        }


        private void _deviceModel_ChannelsChanged(object sender, System.EventArgs e)
        {
            if (_deviceModel.IndexChannel != null)
            {
                BeginInvoke((MethodInvoker)delegate
               {
                   var signalChart1 = new DrawableSignalChart();
                   var signalChart2 = new DrawableSignalChart();

                   signalChart1.SetChannel(_deviceModel.T3O1SignalChannel);
                   signalChart2.SetChannel(_deviceModel.T4O21SignalChannel);
                    //                    _signalViewController[0].SetChannel(_deviceModel.T3O1SignalChannel);
                    //                    _signalViewController[1].SetChannel(_deviceModel.T4O21SignalChannel);

                    _startSignalButton.Enabled = true;
                   _stopSignalButton.Enabled = true;
                   ResistanceCheckButton.Enabled = true;

                   _deviceModel.BatteryChannel.LengthChanged += BatteryChannel_LengthChanged;
                   try
                   {
                       if (_deviceModel.BatteryChannel.TotalLength > 0)
                       {
                           BatteryLabel.Text = $"{_deviceModel.BatteryChannel.ReadData(_deviceModel.BatteryChannel.TotalLength - 1, 1)[0]} %";
                       }
                   }
                   catch { }

                   var indexChart = new EegIndexChart();
                   _indexChartPresenter = new IndexChartPresenter(indexChart, _deviceModel.IndexChannel,
                       _emotionsStartButton, _statesStopButton, this);

                   var spectrumChartT3O1 = new SpectrumChart();
                   var spectrumChartT4O2 = new SpectrumChart();
                   _spectrumChartPresenter = new SpectrumChartPresenter(spectrumChartT3O1, _spectrumAmplitudeTrackBar,
                       new SpectrumModel(_deviceModel.T3O1SpectrumChannel));
                   _spectrumChartPresenter = new SpectrumChartPresenter(spectrumChartT4O2, _spectrumAmplitudeTrackBar,
                       new SpectrumModel(_deviceModel.T4O2SpectrumChannel));

                   var compositeDrawable = new CompoundDrawable();
                   compositeDrawable.AddDrawable(indexChart, new PointF(0f, 0f), new SizeF(0.35f, 0.94f));
                   compositeDrawable.AddDrawable(spectrumChartT3O1, new PointF(0.35f, 0.0f), new SizeF(0.3f, 0.47f));
                   compositeDrawable.AddDrawable(spectrumChartT4O2, new PointF(0.35f, 0.47f), new SizeF(0.3f, 0.47f));
                   compositeDrawable.AddDrawable(_statesChart, new PointF(0f, 0.94f), new SizeF(1.0f, 0.06f));
                   compositeDrawable.AddDrawable(signalChart1, new PointF(0.65f, 0.0f), new SizeF(0.35f, 0.47f));
                   compositeDrawable.AddDrawable(signalChart2, new PointF(0.65f, 0.47f), new SizeF(0.35f, 0.47f));
                   _drawableControl.Drawable = compositeDrawable;

               });
            }
            else
            {
                BeginInvoke((MethodInvoker)delegate
                {
                    _startSignalButton.Enabled = false;
                    _stopSignalButton.Enabled = false;
                    ResistanceCheckButton.Enabled = false;
                });
                _drawableControl.Drawable = new EmptyDrawable();
            }
        }

        private void BatteryChannel_LengthChanged(object sender, int length)
        {
            BeginInvoke((MethodInvoker)delegate
            {
                try
                {
                    if (length > 0)
                    {
                        BatteryLabel.Text = $"{_deviceModel.BatteryChannel.ReadData(length - 1, 1)[0]} %";
                    }
                }
                catch { }
            });
        }

        private void _findDeviceButton_Click(object sender, System.EventArgs e)
        {
            try
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
            catch (Exception exc)
            {
                MessageBox.Show($"Cannot connect to device: {exc.Message}", "Connection error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void _startSignalButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                _deviceModel.StartSignal();
                _indexChartPresenter.OnSignalStarted();
                _signalIsActive = true;
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }
        }

        private void _stopSignalButton_Click(object sender, System.EventArgs e)
        {
            _deviceModel.StopSignal();
            _signalIsActive = false;
        }

        private void _emotionsStartButton_Click(object sender, System.EventArgs e)
        {
            if (_emotionsChartPresenter != null)
                return;

            try
            {
                var channel = _deviceModel.CreateEmotionChannel();
                _emotionsChartPresenter = new EmotionsChartPresenter(
                    _statesChart,
                    channel,
                    _deviceModel.AlphaLeftPowerChannel,
                    _deviceModel.BetaLeftPowerChannel,
                    _deviceModel.AlphaRightPowerChannel,
                    _deviceModel.BetaRightPowerChannel,
                    _deviceModel.IndexChannel
                );
                SetDestination();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }
        }

        private void _statesStopButton_Click(object sender, EventArgs e)
        {
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

        private void ResistanceCheckButton_Click(object sender, EventArgs e)
        {
            try
            {
                var resistanceForm = new ResistanceForm(_deviceModel);
                resistanceForm.ShowDialog();
                if (_signalIsActive)
                {
                    _deviceModel.StartSignal();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show($"Cannot measure resistance: {exc.Message}", "Resistance error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
