namespace CallibriFeatures.Signal
{
    public class HorizontalScan : IHorizontalScan
    {
        public HorizontalScan(int value)
        {
            Seconds = value;
        }

        public int Seconds { get; }

        public override string ToString()
        {
            return $"{Seconds} s";
        }
    }
}