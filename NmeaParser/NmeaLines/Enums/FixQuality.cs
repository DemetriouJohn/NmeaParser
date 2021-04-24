
namespace NmeaParser.NmeaLines.Enums
{
    public enum FixQuality : int
    {
        Invalid = 0,
        GpsFix = 1,
        DgpsFix = 2,
        PpsFix = 3,
        RealTimeKinematic = 4,
        FloatRealTimeKinematic = 5,
        Estimated = 6,
        ManualInput = 7,
        Simulation = 8
    }
}
