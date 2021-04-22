namespace NmeaParser.NmeaLines
{
    public class NmeaMessage
    {
        internal const string RMCCode = "RMC";
        internal const string RMBCode = "RMB";
        internal const string RMACode = "RMA";
        internal const string GGACode = "GGA";
        internal const string VTGCode = "VTG";
        internal const string GllCode = "GLL";
        internal const string GSACode = "GSA";

        internal static readonly NmeaMessage Empty = new NmeaMessage();

        private NmeaMessage()
        {
            IsEmpty = true;
        }

        public NmeaMessage(NmeaType nmeaType)
        {
            IsEmpty = false;
            NmeaType = nmeaType;
        }

        public NmeaType NmeaType { get; }
        public bool IsEmpty { get; }
    }
}