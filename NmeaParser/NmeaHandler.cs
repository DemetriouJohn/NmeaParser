using NmeaParser.NmeaLines;

namespace NmeaParser
{
    public class NmeaHandler : INmeaHandler
    {
        private NmeaLineFactory m_nmeaLineFactory;
        private readonly SystemState m_systemState = new SystemState();
        public ISystemState SystemState => m_systemState;

        private NmeaHandler()
        {
            m_nmeaLineFactory = new NmeaLineFactory();
        }

        public NmeaType ParseLine(string nmeaLine)
        {
            var nmeaMessage = m_nmeaLineFactory.ParseLine(nmeaLine);
            if (nmeaMessage.IsEmpty)
            {
                return NmeaType.BadMessage;
            }

            m_systemState.Handle(nmeaMessage as dynamic);
            return nmeaMessage.NmeaType;
        }

        public static INmeaHandler GetNmeaHandler() => new NmeaHandler();
    }
}
