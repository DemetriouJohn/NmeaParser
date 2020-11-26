using ExtendedGeoCoordinate;
using System;

namespace NmeaParser.NmeaLines
{
    public class GgaLine : NmeaMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GgaLine"/> class.
        /// </summary>
        /// <param name="type">The message type</param>
        /// <param name="message">The NMEA message values.</param>
        public GgaLine(TimeSpan fixTime,
            double latitude,
            double longitude,
            double altitude,
            string altitudeUnits,
            FixQuality quality,
            int numOfSatellites,
            double hdop,
            double geoIdSeparation,
            string geoIdSeparationUnits,
            TimeSpan timeSinceLastUpdate,
            int dgpsStationId) : base(NmeaType.Gga)
        {
            FixTime = fixTime;
            Position = new GeoCoordinate(latitude, longitude, altitude, altitudeUnits);

            Quality = quality;
            NumberOfSatellites = numOfSatellites;
            Hdop = hdop;
            GeoidalSeparation = geoIdSeparation;
            GeoidalSeparationUnits = geoIdSeparationUnits;
            TimeSinceLastDgpsUpdate = timeSinceLastUpdate;
            DgpsStationId = dgpsStationId;
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

        /// <summary>
        /// Fix quality indicater
        /// </summary>
    }
}
