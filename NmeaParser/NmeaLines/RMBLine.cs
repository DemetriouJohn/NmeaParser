using ExtendedGeoCoordinate;
using NmeaParser.NmeaLines.Enums;
using SmartExtensions;
using System.Globalization;

namespace NmeaParser.NmeaLines
{
    public sealed class RmbLine : NmeaMessage
    {
        public RmbLine(string nmeaLine) : base(NmeaType.Rmb, nmeaLine)
        {
            Status = message[0] == "A" ? RmbDataStatus.Ok : RmbDataStatus.Warning;

            if (double.TryParse(message[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var tmp))
            {
                CrossTrackError = tmp;

                if (message[2] == "L") //Steer left
                {
                    CrossTrackError *= -1;
                }
            }
            else
            {
                CrossTrackError = double.NaN;
            }

            message[3].TryToInt32(out var originWaypointId);
            OriginWaypointId = originWaypointId;

            message[4].TryToInt32(out var destinationWaypointId);
            DestinationWaypointId = destinationWaypointId;


            message[9].TryToDouble(out var rangeToDestination);
            message[10].TryToDouble(out var trueBearing);
            RangeToDestination = rangeToDestination;
            TrueBearing = trueBearing;
            message[11].TryToDouble(out var velocity);
            Velocity = velocity;

            Arrived = message[12] == "A";

            var destinationLatitude = Helper.StringToLatitude(message[5], message[6]);
            var destinationLongitude = Helper.StringToLongitude(message[7], message[8]);
            DestinationGeoCoordinate = new GeoCoordinate(destinationLatitude, destinationLongitude);
        }

        /// <summary>
        /// Data Status
        /// </summary>
        public RmbDataStatus Status { get; }

        /// <summary>
        /// Cross-track error (steer left when negative, right when positive)
        /// </summary>
        public double CrossTrackError { get; }

        /// <summary>
        /// Origin waypoint ID
        /// </summary>
        public double OriginWaypointId { get; }

        /// <summary>
        /// Destination waypoint ID
        /// </summary>
        public double DestinationWaypointId { get; }

        /// <summary>
        /// Range to destination in nautical miles
        /// </summary>
        public double RangeToDestination { get; }

        /// <summary>
        /// True bearing to destination
        /// </summary>
        public double TrueBearing { get; }

        /// <summary>
        /// Velocity towards destination in knots
        /// </summary>
        public double Velocity { get; }

        /// <summary>
        /// Arrived (<c>true</c> if arrived)
        /// </summary>
        public bool Arrived { get; }

        /// <summary>
        /// Destination GeoCoordinate position
        /// </summary>
        public GeoCoordinate DestinationGeoCoordinate { get; }
    }
}