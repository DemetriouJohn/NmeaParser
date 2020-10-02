using NmeaParser.RMB;
using NmeaParser.RMC;
using SmartExtensions;
using System;
using System.Globalization;

namespace NmeaParser
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

        public RMBLine ParseRmb(string nmeaLine)
        {
            if (nmeaLine == null || !ValidateLine(nmeaLine))
            {
                throw new ArgumentException("Invalid RMB", nameof(nmeaLine));
            }

            var trimmed = RemoveNmeaDescription(nmeaLine);

            var nmeaValues = trimmed.Split(',');

            var status = nmeaValues[0] == "A" ? RmbDataStatus.Ok : RmbDataStatus.Warning;
            double crossTrackError = double.NaN;
            int originWaypointId, destinationWaypointId;
            if (double.TryParse(nmeaValues[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var tmp))
            {
                crossTrackError = tmp;

                if (nmeaValues[2] == "L") //Steer left
                {
                    crossTrackError *= -1;
                }
            }

            originWaypointId = int.Parse(nmeaValues[3], CultureInfo.InvariantCulture);

            destinationWaypointId = int.Parse(nmeaValues[4], CultureInfo.InvariantCulture);

            var destinationLatitude = Helper.StringToLatitude(nmeaValues[5], nmeaValues[6]);
            var destinationLongitude = Helper.StringToLongitude(nmeaValues[7], nmeaValues[8]);

            double.TryParse(nmeaValues[9], NumberStyles.Float, CultureInfo.InvariantCulture, out var rangeToDestination);
            double.TryParse(nmeaValues[10], NumberStyles.Float, CultureInfo.InvariantCulture, out var trueBearing);
            double.TryParse(nmeaValues[11], NumberStyles.Float, CultureInfo.InvariantCulture, out var velocity);

            var arrived = nmeaValues[12] == "A";
            return new RMBLine(status,
                crossTrackError,
                originWaypointId,
                destinationWaypointId,
                destinationLatitude,
                destinationLongitude,
                rangeToDestination,
                trueBearing,
                velocity,
                arrived);
        }

        public RMCLine ParseRmc(string nmeaLine)
        {
            if (nmeaLine == null || !ValidateLine(nmeaLine))
            {
                throw new ArgumentException("Invalid RMC", "message");
            }

            var trimmed = RemoveNmeaDescription(nmeaLine);
            var nmeaValues = trimmed.Split(',');
            DateTimeOffset fixTime = default;
            if (nmeaValues[8].Length == 6 && nmeaValues[0].Length >= 6)
            {
                fixTime = new DateTimeOffset(int.Parse(nmeaValues[8].Substring(4, 2), CultureInfo.InvariantCulture) + 2000,
                                       int.Parse(nmeaValues[8].Substring(2, 2), CultureInfo.InvariantCulture),
                                       int.Parse(nmeaValues[8].Substring(0, 2), CultureInfo.InvariantCulture),
                                       int.Parse(nmeaValues[0].Substring(0, 2), CultureInfo.InvariantCulture),
                                       int.Parse(nmeaValues[0].Substring(2, 2), CultureInfo.InvariantCulture),
                                       0, TimeSpan.Zero).AddSeconds(double.Parse(nmeaValues[0].Substring(4), CultureInfo.InvariantCulture));
            }

            var active = nmeaValues[1] == "A";
            var latitude = Helper.StringToLatitude(nmeaValues[2], nmeaValues[3]);
            var longitude = Helper.StringToLongitude(nmeaValues[4], nmeaValues[5]);
            nmeaValues[6].TryToDouble(out var speed);
            nmeaValues[7].TryToDouble(out var course);
            nmeaValues[9].TryToDouble(out var magneticVariation);
            if (!double.IsNaN(magneticVariation) && nmeaValues[10] == "W")
            {
                magneticVariation *= -1;
            }

            return new RMCLine(fixTime,
                active,
                latitude,
                longitude,
                speed,
                course,
                magneticVariation);
        }

        private string RemoveNmeaDescription(string nmeaLine)
        {
            return nmeaLine.Substring(7);
        }
    }
}
