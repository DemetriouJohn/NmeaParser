using NmeaParser.NmeaLines;

namespace NmeaParser
{
    public interface INmeaHandler
    {
        NmeaType ParseLine(string nmeaLine);
    }
}