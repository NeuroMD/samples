using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Neuro;

namespace Indices
{
    class IndexListItem : ListViewItem
    {
        private readonly SpectrumPowerChannel _spectrumPowerChannel;
        private readonly Control _context;

        public IndexListItem(Control context, SpectrumPowerChannel spectrumPowerChannel, IEnumerable<string> channels, float lowFreq, float highFreq, double window, double overlap)
        {
            _context = context;
            Text = spectrumPowerChannel.Info.Name;
            SubItems.Add(channels.Aggregate("", (current, chan) => current + chan + " "));
            SubItems.Add($"{lowFreq} - {highFreq} Hz");
            SubItems.Add($"{window}/{overlap}");
            SubItems.Add("0.0");
            _spectrumPowerChannel = spectrumPowerChannel;
            _spectrumPowerChannel.LengthChanged += SpectrumPowerChannelLengthChanged;
        }

        private void SpectrumPowerChannelLengthChanged(object sender, int length)
        {
            var newIndexValue = _spectrumPowerChannel.ReadData(length - 1, 1)[0];
            _context.Invoke((MethodInvoker)delegate
            {
                SubItems[4].Text = (newIndexValue * 1e6).ToString("F4");
            });
        }
    }
}
