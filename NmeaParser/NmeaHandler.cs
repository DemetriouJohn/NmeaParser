namespace NmeaParser
{
    public class NmeaHandler : INmeaHandler
    {
        private NmeaLineFactory m_nmeaLineFactory;
        private readonly SystemState m_systemState = new SystemState();
        public ISystemState SystemState => m_systemState;

        public NmeaHandler()
        {
            m_nmeaLineFactory = new NmeaLineFactory();
        }

        public void ParseLine(string nmeaLine) => m_systemState.Handle(m_nmeaLineFactory.ParseLine(nmeaLine) as dynamic);
    }
}
