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

        private NmeaMessage ParseVtg(string nmeaLine)
        {
            var message = nmeaLine.Split(',');
            message[0].TryToDouble(out var courseTrue);
            message[2].TryToDouble(out var courseMagnetic);
            message[4].TryToDouble(out var speedKnots);
            message[6].TryToDouble(out var speedKph);

            return new VTGLine(courseTrue,
                courseMagnetic,
                speedKnots,
                speedKph);
        }

        private NmeaMessage ParseGga(string nmeaLine)
        {
            var message = nmeaLine.Split(',');
            var fixTime = Helper.StringToTimeSpan(message[0]);
            var latitude = Helper.StringToLatitude(message[1], message[2]);
            var longitude = Helper.StringToLongitude(message[3], message[4]);
            FixQuality quality = message[5].TryToInt32(out var fixQuality) ? (FixQuality)fixQuality : FixQuality.Invalid;

            message[6].TryToInt32(out var numberOfSatellites);
            if (!message[7].TryToDouble(out var hdop))
            {
                return NmeaMessage.Empty;
            }

            if (!message[8].TryToDouble(out var altitude))
            {
                return NmeaMessage.Empty;
            }

            var altitudeUnits = message[9];
            message[10].TryToDouble(out var geoidalSeparation);
            var geoidalSeparationUnits = message[11];
            TimeSpan timeSinceLastUpdate;
            if (message[12].TryToDouble(out var timeInSeconds))
            {
                timeSinceLastUpdate = timeInSeconds.Seconds();
            }
            else
            {
                timeSinceLastUpdate = TimeSpan.MaxValue;
            }

            int dgpsStationId;
            if (message[13].Length > 0)
            {
                dgpsStationId = int.Parse(message[13], CultureInfo.InvariantCulture);
            }
            else
            {
                dgpsStationId = -1;
            }

            return new GgaLine(fixTime,
                latitude,
                longitude,
                altitude,
                altitudeUnits,
                quality,
                numberOfSatellites,
                hdop,
                geoidalSeparation,
                geoidalSeparationUnits,
                timeSinceLastUpdate,
                dgpsStationId);
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

        private RMBLine ParseRmb(string nmeaLine)
        {
            var nmeaValues = nmeaLine.Split(',');

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

        private RMCLine ParseRmc(string nmeaLine)
        {
            var nmeaValues = nmeaLine.Split(',');
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

        private string RemoveConstellationAndChecksum(string nmeaLine)
        {
            var constellationRemoved = nmeaLine.Substring(3);
            return constellationRemoved.Split('*')[0];
        }

        private string RemoveNmeaDescription(string nmeaLine)
        {
            return nmeaLine.Substring(4);
        }
    }
}

