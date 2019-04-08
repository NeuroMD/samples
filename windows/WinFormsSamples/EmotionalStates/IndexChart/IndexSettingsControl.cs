using System;
using System.Windows.Forms;

namespace EmotionalStates.IndexChart
{
    public partial class IndexSettingsControl : UserControl
    {
        public event EventHandler SettingsChanged;

        private double _alphaWeight;
        public double AlphaWeight
        {
            get => _alphaWeight;
            set
            {
                _alphaWeight = value;
                _alphaWeightTextBox.Text = _alphaWeight.ToString("f2");
            }
        }

        private double _betaWeight;
        public double BetaWeight
        {
            get => _betaWeight;
            set
            {
                _betaWeight = value;
                _betaWeightTextBox.Text = _betaWeight.ToString("f2");
            }
        }

        private double _deltaWeight;
        public double DeltaWeight
        {
            get => _deltaWeight;
            set
            {
                _deltaWeight = value;
                _deltaWeightTextBox.Text = _deltaWeight.ToString("f2");
            }
        }

        private double _thetaWeight;
        public double ThetaWeight
        {
            get => _thetaWeight;
            set
            {
                _thetaWeight = value;
                _thetaWeightTextBox.Text = _thetaWeight.ToString("f2");
            }
        }

        private double _delay;
        public double Delay
        {
            get => _delay;
            set
            {
                _delay = value;
                _delayTextBox.Text = _delay.ToString("f2");
            }
        }

        public IndexSettingsControl()
        {
            InitializeComponent();
        }

        private void SettingTextChanged(object sender, EventArgs e)
        {
            _applySettingsButton.Enabled = true;
        }

        private void _applySettingsButton_Click(object sender, EventArgs e)
        {
            try
            {
                _alphaWeight = Convert.ToDouble(_alphaWeightTextBox.Text);
                _betaWeight = Convert.ToDouble(_betaWeightTextBox.Text);
                _deltaWeight = Convert.ToDouble(_deltaWeightTextBox.Text);
                _thetaWeight = Convert.ToDouble(_thetaWeightTextBox.Text);
                _delay = Convert.ToDouble(_delayTextBox.Text);
            }
            catch (FormatException fe)
            {
                MessageBox.Show("Wrong number format", "Format error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                SettingsChanged?.Invoke(this, null);
                _applySettingsButton.Enabled = false;
            }
            catch (Exception exc)
            {
                MessageBox.Show($"Unable apply index settings {exc.Message}", "Apply error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
