namespace NmeaParser
{
    public class RMB
    {
        public RMB(DataStatus status, double crossTrackError, double originWaypointId, double destinationWaypointId, double destinationLatitude, double destinationLongitude, double rangeToDestination, double trueBearing, double velocity, bool arrived)
        {
            Status = status;
            CrossTrackError = crossTrackError;
            OriginWaypointId = originWaypointId;
            DestinationWaypointId = destinationWaypointId;
            DestinationLatitude = destinationLatitude;
            DestinationLongitude = destinationLongitude;
            RangeToDestination = rangeToDestination;
            TrueBearing = trueBearing;
            Velocity = velocity;
            Arrived = arrived;
        }

        /// <summary>
        /// Data Status
        /// </summary>
        public DataStatus Status { get; }

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
        /// Destination Latitude
        /// </summary>
        public double DestinationLatitude { get; }

        /// <summary>
        /// Destination Longitude
        /// </summary>
        public double DestinationLongitude { get; }

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
    }
}