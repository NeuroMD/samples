using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Neuro;

namespace Biofeedback
{
    class IndexListItem : ListViewItem
    {
        private readonly EegIndexChannel _indexChannel;
        private readonly Control _context;

        public IndexListItem(Control context, EegIndexChannel indexChannel, EegIndex index, IEnumerable<string> channels)
        {
            _context = context;
            Text = index.Name;
            SubItems.Add(channels.Aggregate("", (current, chan) => current + chan + " "));
            SubItems.Add($"{index.FrequencyBottom} - {index.FrequencyTop} Hz");
            SubItems.Add("0.0");
            _indexChannel = indexChannel;
            _indexChannel.LengthChanged += _indexChannel_LengthChanged;
        }

        private void _indexChannel_LengthChanged(object sender, int length)
        {
            var newIndexValue = _indexChannel.ReadData(length - 1, 1)[0];
            _context.Invoke((MethodInvoker)delegate
            {
                SubItems[3].Text = newIndexValue.ToString("F4");
            });
        }
    }
}
