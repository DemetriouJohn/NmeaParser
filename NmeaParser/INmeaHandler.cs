namespace NmeaParser
{
    public interface INmeaHandler
    {
        void ParseLine(string nmeaLine);
    }
}