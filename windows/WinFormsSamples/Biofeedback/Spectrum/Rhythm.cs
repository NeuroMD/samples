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

        public EEGRhythm(string name, double frequencyBottom, double frequencyTop, Color color, string symbol)
        {
            Name = name;
            Symbol = symbol;
            FreqBegin = frequencyBottom;
            FreqEnd = frequencyTop;
            Color = color;
        }

		public override string ToString()
		{
			return Name;
		}
    }
}
