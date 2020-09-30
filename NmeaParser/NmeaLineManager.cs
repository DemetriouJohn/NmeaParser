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

        public RMB ParseRmb(string nmeaLine)
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
            return new RMB(status,
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

        private string RemoveNmeaDescription(string nmeaLine)
        {
            return nmeaLine.Substring(7);
        }
    }
}
