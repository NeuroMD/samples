namespace Indices.Spectrum
{
    public class Spectrum
    {
        public string Name { get; }
        public double[] Data { get; }

        public Spectrum(string name, double[] data)
        {
            Name = name;
            Data = data;
        }
    }
}
