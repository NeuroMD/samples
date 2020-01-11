using System;
using System.Windows.Forms;
using Neuro;

namespace CallibriFeatures.Devices
{
    public partial class MEMSControl : UserControl
    {
        public MEMSControl()
        {
            InitializeComponent();

            GyroscopeSensComboBox.Items.Add(GyroscopeSensitivity.Sens250Grad);
            GyroscopeSensComboBox.Items.Add(GyroscopeSensitivity.Sens500Grad);
            GyroscopeSensComboBox.Items.Add(GyroscopeSensitivity.Sens1000Grad);
            GyroscopeSensComboBox.Items.Add(GyroscopeSensitivity.Sens2000Grad);
            GyroscopeSensComboBox.SelectedItem = GyroscopeSensitivity.Sens250Grad;

            AccelerometerSensComboBox.Items.Add(AccelerometerSensitivity.Sens2g);
            AccelerometerSensComboBox.Items.Add(AccelerometerSensitivity.Sens4g);
            AccelerometerSensComboBox.Items.Add(AccelerometerSensitivity.Sens8g);
            AccelerometerSensComboBox.Items.Add(AccelerometerSensitivity.Sens16g);
            AccelerometerSensComboBox.SelectedItem = AccelerometerSensitivity.Sens2g;

            GyroscopeSensComboBox.SelectedIndexChanged += GyroscopeSensComboBox_SelectedIndexChanged;
            AccelerometerSensComboBox.SelectedIndexChanged += AccelerometerSensComboBox_SelectedIndexChanged;

            StartMemsButton.Click += StartMemsButton_Click;
            StopMemsButton.Click += StopMemsButton_Click;
            StartOrientationButton.Click += StartOrientationButton_Click;
            StopOrientationButton.Click += StopOrientationButton_Click;
            CalibrateMemsButton.Click += CalibrateMemsButton_Click;
            ResetQuternionButton.Click += ResetQuternionButton_Click;
        }

        private void ResetQuternionButton_Click(object sender, EventArgs e)
        {
            try
            {
                ResetQuaternionButtonClicked?.Invoke(this, null);
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Cannot reset quaternion: {exception.Message}", "MEMS error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void CalibrateMemsButton_Click(object sender, EventArgs e)
        {
            try
            {
                CalibrateMemsButtonClicked?.Invoke(this, null);
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Cannot start MEMS calibration: {exception.Message}", "MEMS error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void StopOrientationButton_Click(object sender, EventArgs e)
        {
            try
            {
                StopOrientButtonClicked?.Invoke(this, null);
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Cannot stop orientation: {exception.Message}", "MEMS error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void StartOrientationButton_Click(object sender, EventArgs e)
        {
            try
            {
                StartOrientButtonClicked?.Invoke(this, null);
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Cannot start orientation: {exception.Message}", "MEMS error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void StopMemsButton_Click(object sender, EventArgs e)
        {
            try
            {
                StopMemsButtonClicked?.Invoke(this, null);
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Cannot stop MEMS: {exception.Message}", "MEMS error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void StartMemsButton_Click(object sender, EventArgs e)
        {
            try
            {
                StartMemsButtonClicked?.Invoke(this, null);
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Cannot start MEMS: {exception.Message}", "MEMS error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void AccelerometerSensComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedAccelerometerSensChanged?.Invoke(this, (AccelerometerSensitivity)AccelerometerSensComboBox.SelectedItem);
        }

        private void GyroscopeSensComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedGyroscopeSensChanged?.Invoke(this, (GyroscopeSensitivity)GyroscopeSensComboBox.SelectedItem);
        }

        public event EventHandler<GyroscopeSensitivity> SelectedGyroscopeSensChanged;
        public event EventHandler<AccelerometerSensitivity> SelectedAccelerometerSensChanged;

        public event EventHandler StartMemsButtonClicked;
        public event EventHandler StopMemsButtonClicked;
        public event EventHandler StartOrientButtonClicked;
        public event EventHandler StopOrientButtonClicked;
        public event EventHandler ResetQuaternionButtonClicked;
        public event EventHandler CalibrateMemsButtonClicked;

        public GyroscopeSensitivity GyroscopeSensitivityComboValue
        {
            set
            {
                if (InvokeRequired)
                {
                    BeginInvoke((MethodInvoker)delegate { GyroscopeSensComboBox.SelectedItem = value; });
                }
                else
                {
                    GyroscopeSensComboBox.SelectedItem = value;
                }
            }
        }

        public AccelerometerSensitivity AccelerometerSensitivityComboValue
        {
            set
            {
                if (InvokeRequired)
                {
                    BeginInvoke((MethodInvoker)delegate { AccelerometerSensComboBox.SelectedItem = value; });
                }
                else
                {
                    AccelerometerSensComboBox.SelectedItem = value;
                }
            }
        }

        public bool CalibrationProgressBarVisible
        {
            set
            {
                if (InvokeRequired)
                {
                    BeginInvoke((MethodInvoker)delegate { CalibrationProgressBar.Visible = value; });
                }
                else
                {
                    CalibrationProgressBar.Visible = value;
                }
            }
        }

        public void SetEnabled(bool enabled)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate { Enabled = enabled; });
            }
            else
            {
                Enabled = enabled;
            }
        }
    }
}
