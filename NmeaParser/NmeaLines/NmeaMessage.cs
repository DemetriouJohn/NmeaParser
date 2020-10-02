namespace NmeaParser.NmeaLines
{
    public class NmeaMessage
    {
        internal const string RMCCode = "RMC";

        internal const string RMBCode = "RMB";

        public NmeaMessage(NmeaType nmeaType)
        {
            NmeaType = nmeaType;
        }

        public NmeaType NmeaType { get; }
    }
}