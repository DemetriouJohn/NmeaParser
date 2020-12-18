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
        public RmcLine(string nmeaLine)
            : base(NmeaType.Rmc)
        {
            var nmeaValues = nmeaLine.Split(',');

            if (nmeaValues[8].Length == 6 && nmeaValues[0].Length >= 6)
            {
                FixTime = new DateTime(int.Parse(nmeaValues[8].Substring(4, 2), CultureInfo.InvariantCulture) + 2000,
                                       int.Parse(nmeaValues[8].Substring(2, 2), CultureInfo.InvariantCulture),
                                       int.Parse(nmeaValues[8].Substring(0, 2), CultureInfo.InvariantCulture),
                                       int.Parse(nmeaValues[0].Substring(0, 2), CultureInfo.InvariantCulture),
                                       int.Parse(nmeaValues[0].Substring(2, 2), CultureInfo.InvariantCulture),
                                       int.Parse(nmeaValues[0].Substring(4), CultureInfo.InvariantCulture));
            }
            else
            {
                FixTime = default;
            }

            Active = nmeaValues[1] == "A";

            var latitude = Helper.StringToLatitude(nmeaValues[2], nmeaValues[3]);
            var longitude = Helper.StringToLongitude(nmeaValues[4], nmeaValues[5]);
            nmeaValues[6].TryToDouble(out var speed);
            nmeaValues[7].TryToDouble(out var course);
            GeoCoordinate = new GeoCoordinate(latitude, longitude, 0, 0, 0, speed, course);

            nmeaValues[9].TryToDouble(out var magneticVariation);
            if (!double.IsNaN(magneticVariation) && nmeaValues[10] == "W")
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
