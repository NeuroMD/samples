namespace CallibriFeatures.Signal
{
    public interface IVerticalScan
    {
        int MicroVolts { get; }
    }

    public interface IHorizontalScan
    {
        int Seconds { get; }
    }
}