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

        internal static readonly NmeaMessage Empty = new NmeaMessage();

        private NmeaMessage()
        {
            IsEmpty = true;
        }

        public NmeaMessage(NmeaType nmeaType, string nmeaLine)
        {
            IsEmpty = false;
            NmeaType = nmeaType;
            message = nmeaLine.Split(',');
        }

        public NmeaType NmeaType { get; }

        protected readonly string[] message;

        public bool IsEmpty { get; }
    }
}