using ExtendedGeoCoordinate;
using SmartExtensions;
using System;
using System.Globalization;

namespace NmeaParser.NmeaLines
{
    public sealed class RmcLine : NmeaMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Rmc"/> class.
        /// </summary>
        /// <param name="type">The message type</param>
        /// <param name="message">The NMEA message values.</param>
        public RmcLine(string nmeaLine) : base(NmeaType.Rmc, nmeaLine)
        {
            if (message[8].Length == 6 && message[0].Length >= 6)
            {
                FixTime = new DateTime(int.Parse(message[8].Substring(4, 2), CultureInfo.InvariantCulture) + 2000,
                                       int.Parse(message[8].Substring(2, 2), CultureInfo.InvariantCulture),
                                       int.Parse(message[8].Substring(0, 2), CultureInfo.InvariantCulture),
                                       int.Parse(message[0].Substring(0, 2), CultureInfo.InvariantCulture),
                                       int.Parse(message[0].Substring(2, 2), CultureInfo.InvariantCulture),
                                       int.Parse(message[0].Substring(4), CultureInfo.InvariantCulture));
            }
            else
            {
                FixTime = default;
            }

            Active = message[1] == "A";

            var latitude = Helper.StringToLatitude(message[2], message[3]);
            var longitude = Helper.StringToLongitude(message[4], message[5]);
            message[6].TryToDouble(out var speed);
            message[7].TryToDouble(out var course);
            GeoCoordinate = new GeoCoordinate(latitude, longitude, 0, 0, 0, speed, course);

            message[9].TryToDouble(out var magneticVariation);
            if (!double.IsNaN(magneticVariation) && message[10] == "W")
            {
                magneticVariation *= -1;
            }

            MagneticVariation = magneticVariation;
        }

        /// <summary>
        /// Fix Time
        /// </summary>
        public DateTime FixTime { get; }

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
