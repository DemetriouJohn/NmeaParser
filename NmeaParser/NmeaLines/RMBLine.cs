using System.Globalization;
using ExtendedGeoCoordinate;
using SmartExtensions;

namespace NmeaParser.NmeaLines
{
    public class RMBLine : NmeaMessage
    {
        public RMBLine(string nmeaLine) : base(NmeaType.Rmb)
        {
            var nmeaValues = nmeaLine.Split(',');

            Status = nmeaValues[0] == "A" ? RmbDataStatus.Ok : RmbDataStatus.Warning;

            if (double.TryParse(nmeaValues[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var tmp))
            {
                CrossTrackError = tmp;

                if (nmeaValues[2] == "L") //Steer left
                {
                    CrossTrackError *= -1;
                }
            }
            else
            {
                CrossTrackError = double.NaN;
            }

            nmeaValues[3].TryToInt32(out var originWaypointId);
            OriginWaypointId = originWaypointId;

            nmeaValues[4].TryToInt32(out var destinationWaypointId);
            DestinationWaypointId = destinationWaypointId;


            nmeaValues[9].TryToDouble(out var rangeToDestination);
            nmeaValues[10].TryToDouble(out var trueBearing);
            RangeToDestination = rangeToDestination;
            TrueBearing = trueBearing;
            nmeaValues[11].TryToDouble(out var velocity);
            Velocity = velocity;

            Arrived = nmeaValues[12] == "A";

            var destinationLatitude = Helper.StringToLatitude(nmeaValues[5], nmeaValues[6]);
            var destinationLongitude = Helper.StringToLongitude(nmeaValues[7], nmeaValues[8]);
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