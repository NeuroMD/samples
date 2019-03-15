using System.Drawing;
using Neuro;

namespace Indices.Spectrum
{
  public class EEGRhythm
    {
        public string Name;
        public string Symbol;
        public double FreqBegin;
        public double FreqEnd;
        public Color Color;

        public EEGRhythm(EegIndex index, Color color, string symbol)
        {
            Name = index.Name;
            Symbol = symbol;
            FreqBegin = index.FrequencyBottom;
            FreqEnd = index.FrequencyTop;
            Color = color;
        }

		public override string ToString()
		{
			return Name;
		}
    }
}
