using NmeaParser.NmeaLines;
using SmartExtensions;
using System;
using System.Globalization;

namespace NmeaParser
{
    public class NmeaLineFactory
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

        public NmeaMessage ParseLine(string nmeaLine)
        {
            if (nmeaLine == null || !ValidateLine(nmeaLine))
            {
                throw new ArgumentException("Invalid NMEA Line", nameof(nmeaLine));
            }

            var trimmed = RemoveConstellationAndChecksum(nmeaLine);

            var nmeaType = GetNmeaType(trimmed);

            trimmed = RemoveNmeaDescription(trimmed);
            switch (nmeaType)
            {

                case NmeaType.Rma:
                    break;
                case NmeaType.Rmb:
                    return ParseRmb(trimmed);
                case NmeaType.Rmc:
                    return ParseRmc(trimmed);
                case NmeaType.Gga:
                    return ParseGga(trimmed);
                case NmeaType.Vtg:
                    return ParseVtg(trimmed);
                default:
                    throw new ArgumentOutOfRangeException(nameof(nmeaType));
            }

            return default;
        }

        private NmeaType GetNmeaType(string trimmed)
        {
            if (trimmed.StartsWith(NmeaMessage.RMACode))
            {
                return NmeaType.Rma;
            }
            else if (trimmed.StartsWith(NmeaMessage.RMBCode))
            {
                return NmeaType.Rmb;
            }
            else if (trimmed.StartsWith(NmeaMessage.RMCCode))
            {
                return NmeaType.Rmc;
            }
            else if (trimmed.StartsWith(NmeaMessage.GGACode))
            {
                return NmeaType.Gga;
            }

            return default;
        }

        private string RemoveConstellationAndChecksum(string nmeaLine)
        {
            var constellationRemoved = nmeaLine.Substring(3);
            return constellationRemoved.Split('*')[0];
        }

        private string RemoveNmeaDescription(string nmeaLine)
        {
            return nmeaLine.Substring(4);
        }

        private NmeaMessage ParseVtg(string nmeaLine) => new VtgLine(nmeaLine);

        private NmeaMessage ParseGga(string nmeaLine) => new GgaLine(nmeaLine);

        private RmbLine ParseRmb(string nmeaLine) => new RmbLine(nmeaLine);

        private RmcLine ParseRmc(string nmeaLine) => new RmcLine(nmeaLine);
    }
}
