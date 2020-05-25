using System;

namespace Nmea_Parser
{
    public class NmeaLineManager
    {
        internal bool ValidateLine(string nmeaLine)
        {
            nmeaLine = nmeaLine.Trim();
            if (!nmeaLine.StartsWith("$"))
            {
                return false;
            }

            return ValidateChecksum(nmeaLine.TrimStart('$'));
        }

        internal bool ValidateChecksum(string nmeaLine)
        {
            int checksum = Convert.ToByte(nmeaLine[nmeaLine.IndexOf('$') + 1]);
            for (int i = nmeaLine.IndexOf('$') + 2; i < nmeaLine.IndexOf('*'); i++)
            {
                checksum ^= Convert.ToByte(nmeaLine[i]);
            }
            int givenChecksum = Convert.ToInt16(nmeaLine.Split('*')[1], 16);
            return checksum == givenChecksum;
        }
    }
}
