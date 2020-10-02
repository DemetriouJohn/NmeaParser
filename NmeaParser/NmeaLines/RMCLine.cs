using ExtendedGeoCoordinate;
using System;

namespace NmeaParser.NmeaLines
{
    public class RMCLine : NmeaMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Rmc"/> class.
        /// </summary>
        /// <param name="type">The message type</param>
        /// <param name="message">The NMEA message values.</param>
        public RMCLine(DateTimeOffset fixTime,
            bool active,
            double latitude,
            double longitude,
            double speed,
            double course,
            double magneticVariation)
            : base(NmeaType.Rmc)
        {
            FixTime = fixTime;
            Active = active;
            MagneticVariation = magneticVariation;
            GeoCoordinate = new GeoCoordinate(latitude, longitude, 0, 0, 0, speed, course);
        }

        /// <summary>
        /// Fix Time
        /// </summary>
        public DateTimeOffset FixTime { get; }

        /// <summary>
        /// Gets a value whether the device is active
        /// </summary>
        public bool Active { get; }

        /// <summary>
        /// Magnetic Variation
        /// </summary>
        public double MagneticVariation { get; }

        /// <summary>
        /// Position
        /// </summary>
        public GeoCoordinate GeoCoordinate { get; }
    }
}
