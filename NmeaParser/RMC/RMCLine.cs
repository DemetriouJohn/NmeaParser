using System;

namespace NmeaParser.RMC
{
    public class RMCLine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Rmc"/> class.
        /// </summary>
        /// <param name="type">The message type</param>
        /// <param name="message">The NMEA message values.</param>
        public RMCLine(DateTimeOffset fixTime, bool active, double latitude, double longitude, double speed, double course, double magneticVariation)
        {
            FixTime = fixTime;
            Active = active;
            Latitude = latitude;
            Longitude = longitude;
            Speed = speed;
            Course = course;
            MagneticVariation = magneticVariation;
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
        /// Latitude
        /// </summary>
        public double Latitude { get; }

        /// <summary>
        /// Longitude
        /// </summary>
        public double Longitude { get; }

        /// <summary>
        /// Speed over the ground in knots
        /// </summary>
        public double Speed { get; }

        /// <summary>
        /// Track angle in degrees True
        /// </summary>
        public double Course { get; }

        /// <summary>
        /// Magnetic Variation
        /// </summary>
        public double MagneticVariation { get; }
    }
}
