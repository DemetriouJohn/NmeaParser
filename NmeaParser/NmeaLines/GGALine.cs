using ExtendedGeoCoordinate;
using SmartExtensions;
using System;
using System.Globalization;

namespace NmeaParser.NmeaLines
{
    public class GgaLine : NmeaMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GgaLine"/> class.
        /// </summary>
        /// <param name="type">The message type</param>
        /// <param name="message">The NMEA message values.</param>
        public GgaLine(string nmeaLine) : base(NmeaType.Gga)
        {
            var message = nmeaLine.Split(',');
            FixTime = Helper.StringToTimeSpan(message[0]);
            
            var latitude = Helper.StringToLatitude(message[1], message[2]);
            var longitude = Helper.StringToLongitude(message[3], message[4]);
            message[8].TryToDouble(out var altitude);
            var altitudeUnits = message[9];
            Position = new GeoCoordinate(latitude, longitude, altitude, altitudeUnits);
            
            FixQuality quality = message[5].TryToInt32(out var fixQuality) ? (FixQuality)fixQuality : FixQuality.Invalid;
            Quality = quality;

            message[7].TryToDouble(out var hdop);
            Hdop = hdop;
            
            if (message[12].TryToDouble(out var timeInSeconds))
            {
                TimeSinceLastDgpsUpdate = timeInSeconds.Seconds();
            }
            else
            {
                TimeSinceLastDgpsUpdate = TimeSpan.MaxValue;
            }

            if (message[13].Length > 0)
            {
                DgpsStationId = int.Parse(message[13], CultureInfo.InvariantCulture);
            }
            else
            {
                DgpsStationId = -1;
            }


            message[6].TryToInt32(out var numberOfSatellites);
            NumberOfSatellites = numberOfSatellites;

            message[10].TryToDouble(out var geoidalSeparation);
            GeoidalSeparation = geoidalSeparation;

            var geoidalSeparationUnits = message[11];
            GeoidalSeparationUnits = geoidalSeparationUnits;
        }

        /// <summary>
        /// Time of day fix was taken
        /// </summary>
        public TimeSpan FixTime { get; }

        /// <summary>
        /// Latitude
        /// </summary>
        public GeoCoordinate Position { get; }

        /// <summary>
        /// Fix Quality
        /// </summary>
        public FixQuality Quality { get; }

        /// <summary>
        /// Number of satellites being tracked
        /// </summary>
        public int NumberOfSatellites { get; }

        /// <summary>
        /// Horizontal Dilution of Precision
        /// </summary>
        public double Hdop { get; }

        /// <summary>
        /// Altitude units ('M' for Meters)
        /// </summary>
        public string AltitudeUnits { get; }

        /// <summary>
        /// Geoidal separation: the difference between the WGS-84 earth ellipsoid surface and mean-sea-level (geoid) surface.
        /// </summary>
        /// <remarks>
        /// A negative value means mean-sea-level surface is below the WGS-84 ellipsoid surface.
        /// </remarks>
        /// <seealso cref="GeoidalSeparationUnits"/>
        public double GeoidalSeparation { get; }

        /// <summary>
        /// Altitude units ('M' for Meters)
        /// </summary>
        public string GeoidalSeparationUnits { get; }

        /// <summary>
        /// Time since last DGPS update (ie age of the differential GPS data)
        /// </summary>
        public TimeSpan TimeSinceLastDgpsUpdate { get; }

        /// <summary>
        /// Differential Reference Station ID
        /// </summary>
        public int DgpsStationId { get; }
    }
}
