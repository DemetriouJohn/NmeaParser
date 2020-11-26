using NmeaParser.NmeaLines;

namespace NmeaParser
{
    public interface INmeaHandler
    {
        ISystemState SystemState { get; }

        NmeaType ParseLine(string nmeaLine);
    }
}