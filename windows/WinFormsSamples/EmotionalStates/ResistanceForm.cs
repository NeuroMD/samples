using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Neuro;

namespace EmotionalStates
{
    internal partial class ResistanceForm : Form
    {
        private readonly DeviceModel _model;
        private readonly IDictionary<string, ResistanceChannel> _resistanceChannels;

        public ResistanceForm(DeviceModel model)
        {
            InitializeComponent();
            _model = model;
            _resistanceChannels = model.CreateResistanceChannels();
            _resistanceChannels["T3"].LengthChanged += (sender, length) =>
            {
                if (length < 0) return;
                ShowResistanceOnLabel(T3ResistanceLabel, _resistanceChannels["T3"]);
            };
            _resistanceChannels["T4"].LengthChanged += (sender, length) =>
            {
                if (length < 0) return;
                ShowResistanceOnLabel(T4ResistanceLabel, _resistanceChannels["T4"]);
            };
            _resistanceChannels["O1"].LengthChanged += (sender, length) =>
            {
                if (length < 0) return;
                ShowResistanceOnLabel(O1ResistanceLabel, _resistanceChannels["O1"]);
            };
            _resistanceChannels["O2"].LengthChanged += (sender, length) =>
            {
                if (length < 0) return;
                ShowResistanceOnLabel(O2ResistanceLabel, _resistanceChannels["O2"]);
            };
        }

        private void ResistanceForm_Shown(object sender, EventArgs e)
        {
            _model?.StartResist();
        }

        private void ShowResistanceOnLabel(Label label, ResistanceChannel channel)
        {
            try
            {
                var resistanceOhms = channel.ReadData(channel.TotalLength - 1, 1)[0];
                BeginInvoke((MethodInvoker)delegate { label.Text = $"{channel.Info.Name}: {resistanceOhms / 1000} k"; });
            }
            catch { }
        }

        private void ResistanceForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _model?.StopResist();
            foreach (var resistanceChannel in _resistanceChannels.Values)
            {
                resistanceChannel.Dispose();
            }
        }
    }
}
