using System;
using System.Windows.Forms;

namespace EmotionalStates.EmotionsChart
{
    public partial class EmotionCoefficientsControl : UserControl
    {

        public event EventHandler CoefficientsChanged;

        private double _px1;
        public double PX1
        {
            get => _px1;
            set
            {
                _px1 = value;
                _px1TextBox.Text = _px1.ToString("f2");
            }
        }

        private double _px2;
        public double PX2
        {
            get => _px2;
            set
            {
                _px2 = value;
                _px2TextBox.Text = _px2.ToString("f2");
            }
        }

        private double _px3;
        public double PX3
        {
            get => _px3;
            set
            {
                _px3 = value;
                _px3TextBox.Text = _px3.ToString("f2");
            }
        }

        private double _px4;
        public double PX4
        {
            get => _px4;
            set
            {
                _px4 = value;
                _px4TextBox.Text = _px4.ToString("f2");
            }
        }

        private double _nx1;
        public double NX1
        {
            get => _nx1;
            set
            {
                _nx1 = value;
                _nx1TextBox.Text = _nx1.ToString("f2");
            }
        }

        private double _nx2;
        public double NX2
        {
            get => _nx2;
            set
            {
                _nx2 = value;
                _nx2TextBox.Text = _nx2.ToString("f2");
            }
        }

        private double _nx3;
        public double NX3
        {
            get => _nx3;
            set
            {
                _nx3 = value;
                _nx3TextBox.Text = _nx3.ToString("f2");
            }
        }

        private double _nx4;
        public double NX4
        {
            get => _nx4;
            set
            {
                _nx4 = value;
                _nx4TextBox.Text = _nx4.ToString("f2");
            }
        }

        private double _py1;
        public double PY1
        {
            get => _py1;
            set
            {
                _py1 = value;
                _py1TextBox.Text = _py1.ToString("f2");
            }
        }

        private double _py2;
        public double PY2
        {
            get => _py2;
            set
            {
                _py2 = value;
                _py2TextBox.Text = _py2.ToString("f2");
            }
        }

        private double _py3;
        public double PY3
        {
            get => _py3;
            set
            {
                _py3 = value;
                _py3TextBox.Text = _py3.ToString("f2");
            }
        }

        private double _py4;
        public double PY4
        {
            get => _py4;
            set
            {
                _py4 = value;
                _py4TextBox.Text = _py4.ToString("f2");
            }
        }

        private double _ny1;
        public double NY1
        {
            get => _ny1;
            set
            {
                _ny1 = value;
                _ny1TextBox.Text = _ny1.ToString("f2");
            }
        }

        private double _ny2;
        public double NY2
        {
            get => _ny2;
            set
            {
                _ny2 = value;
                _ny2TextBox.Text = _ny2.ToString("f2");
            }
        }

        private double _ny3;
        public double NY3
        {
            get => _ny3;
            set
            {
                _ny3 = value;
                _ny3TextBox.Text = _ny3.ToString("f2");
            }
        }

        private double _ny4;
        public double NY4
        {
            get => _ny4;
            set
            {
                _ny4 = value;
                _ny4TextBox.Text = _ny4.ToString("f2");
            }
        }

        public EmotionCoefficientsControl()
        {
            InitializeComponent();
        }

        private void CoefficientTextChanged(object sender, EventArgs e) => _applyEmotionCoeffsButton.Enabled = true;

        private void _applyEmotionCoeffsButton_Click(object sender, EventArgs e)
        {
            try
            {
                _px1 = Convert.ToDouble(_px1TextBox.Text);
                _px2 = Convert.ToDouble(_px2TextBox.Text);
                _px3 = Convert.ToDouble(_px3TextBox.Text);
                _px4 = Convert.ToDouble(_px4TextBox.Text);
                _nx1 = Convert.ToDouble(_nx1TextBox.Text);
                _nx2 = Convert.ToDouble(_nx2TextBox.Text);
                _nx3 = Convert.ToDouble(_nx3TextBox.Text);
                _nx4 = Convert.ToDouble(_nx4TextBox.Text);
                _py1 = Convert.ToDouble(_py1TextBox.Text);
                _py2 = Convert.ToDouble(_py2TextBox.Text);
                _py3 = Convert.ToDouble(_py3TextBox.Text);
                _py4 = Convert.ToDouble(_py4TextBox.Text);
                _ny1 = Convert.ToDouble(_ny1TextBox.Text);
                _ny2 = Convert.ToDouble(_ny2TextBox.Text);
                _ny3 = Convert.ToDouble(_ny3TextBox.Text);
                _ny4 = Convert.ToDouble(_ny4TextBox.Text);
            }
            catch (FormatException fe)
            {
                MessageBox.Show("Wrong coefficient format", "Format error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                CoefficientsChanged?.Invoke(this, null);
                _applyEmotionCoeffsButton.Enabled = false;
            }
            catch (Exception exc)
            {
                MessageBox.Show($"Unable apply coefficients values {exc.Message}", "Apply error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
