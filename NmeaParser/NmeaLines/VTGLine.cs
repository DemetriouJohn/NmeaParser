namespace NmeaParser.NmeaLines
{
    public class VTGLine : NmeaMessage
    {
        public VTGLine(double courseTrue,
            double courseMagnetic,
            double speedKnots,
            double speedKph) : base(NmeaType.Vtg)
        {
            CourseTrue = courseTrue;
            CourseMagnetic = courseMagnetic;
            SpeedKnots = speedKnots;
            SpeedKph = speedKph;
        }

        public double CourseTrue { get; }
        public double CourseMagnetic { get; }
        public double SpeedKnots { get; }
        public double SpeedKph { get; }
    }
}
