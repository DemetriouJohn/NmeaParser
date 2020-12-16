using SmartExtensions;

namespace NmeaParser.NmeaLines
{
    public sealed class VtgLine : NmeaMessage
    {
        public VtgLine(string nmeaLine)
            : base(NmeaType.Vtg)
        {
            var message = nmeaLine.Split(',');
            message[0].TryToDouble(out var courseTrue);
            CourseTrue = courseTrue;
            message[2].TryToDouble(out var courseMagnetic);
            CourseMagnetic = courseMagnetic;
            message[4].TryToDouble(out var speedKnots);
            SpeedKnots = speedKnots;
            message[6].TryToDouble(out var speedKph);
            SpeedKph = speedKph;
        }

        public double CourseTrue { get; }
        public double CourseMagnetic { get; }
        public double SpeedKnots { get; }
        public double SpeedKph { get; }
    }
}
