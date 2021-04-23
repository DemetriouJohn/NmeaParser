using ExtendedGeoCoordinate;
using NmeaParser.NmeaLines;
using NmeaParser.NmeaLines.Enums;
using System;

namespace NmeaParser
{
    internal sealed class SystemState : ISystemState
    {
        /// <summary>
        /// The Latest RMB line parsed
        /// </summary>
        public RmbLine Rmb { get; private set; }
        /// <summary>
        /// The Latest RMC line parsed
        /// </summary>
        public RmcLine Rmc { get; private set; }
        /// <summary>
        /// The Latest GGA line parsed
        /// </summary>
        public GgaLine Gga { get; private set; }
        /// <summary>
        /// The Latest VTG line parsed
        /// </summary>
        public VtgLine Vtg { get; private set; }
        /// <summary>
        /// Velocity towards destination in knots
        /// </summary>
        public GllLine Gll { get; private set; }
        public GsaLine Gsa { get; private set; }
        /// <summary>
        /// Current Position
        /// </summary>
        public GeoCoordinate CurrentPosition { get; private set; }
        public GllMode GllMode { get; private set; }
        /// <summary>
        /// Magnetic Variation
        /// </summary>
        public double MagneticVariation { get; private set; }
        /// <summary>
        /// Fix Time
        /// </summary>
        public DateTime FixTime { get; private set; }
        /// <summary>
        /// System Speed
        /// </summary>
        public double Speed => CurrentPosition.Speed;
        public double VelocityInKnots { get; private set; }
        /// <summary>
        /// System Course
        /// </summary>
        public double Course => CurrentPosition.Course;
        public double HDop { get; private set; } = double.NaN;
        public double VDop { get; private set; } = double.NaN;
        public double PDop { get; private set; } = double.NaN;
        public int[] SatelliteIds { get; private set; }
        public ModeSelection GsaMode { get; private set; }
        public FixType FixType { get; private set; }

        internal void Handle(RmbLine nmeaMessage)
        {
            Rmb = nmeaMessage;
            VelocityInKnots = nmeaMessage.Velocity;
        }

        internal void Handle(VtgLine nmeaMessage)
        {
            Vtg = nmeaMessage;
            VelocityInKnots = nmeaMessage.SpeedKnots;
            CurrentPosition = new GeoCoordinate(CurrentPosition.Latitude,
                CurrentPosition.Longitude,
                CurrentPosition.Altitude,
                CurrentPosition.AltitudeUnits,
                CurrentPosition.HorizontalAccuracy,
                CurrentPosition.VerticalAccuracy,
                nmeaMessage.SpeedKph,
                nmeaMessage.CourseTrue);
        }

        internal void Handle(GgaLine nmeaMessage)
        {
            Gga = nmeaMessage;
            CurrentPosition = new GeoCoordinate(nmeaMessage.Position.Latitude,
                nmeaMessage.Position.Longitude,
                nmeaMessage.Position.Altitude,
                nmeaMessage.Position.AltitudeUnits,
                CurrentPosition.HorizontalAccuracy,
                CurrentPosition.VerticalAccuracy,
                CurrentPosition.Speed,
                CurrentPosition.Course);
            HDop = nmeaMessage.Hdop;
        }

        internal void Handle(RmcLine nmeaMessage)
        {
            Rmc = nmeaMessage;
            CurrentPosition = nmeaMessage.GeoCoordinate;
            MagneticVariation = nmeaMessage.MagneticVariation;
            FixTime = nmeaMessage.FixTime;
        }

        internal void Handle(GllLine nmeaMessage)
        {
            Gll = nmeaMessage;
            CurrentPosition = new GeoCoordinate(
                nmeaMessage.Position.Latitude,
                nmeaMessage.Position.Longitude,
                CurrentPosition.Altitude,
                CurrentPosition.AltitudeUnits,
                CurrentPosition.HorizontalAccuracy,
                CurrentPosition.VerticalAccuracy,
                CurrentPosition.Speed,
                CurrentPosition.Course);

            GllMode = nmeaMessage.Mode;
            FixTime = nmeaMessage.FixTime;
        }

        internal void Handle(GsaLine nmeaMessage)
        {
            Gsa = nmeaMessage;
            HDop = nmeaMessage.Hdop;
            VDop = nmeaMessage.Vdop;
            PDop = nmeaMessage.Pdop;
            SatelliteIds = nmeaMessage.SatelliteIDs;
            GsaMode = nmeaMessage.Mode;
            FixType = nmeaMessage.Fix;
        }
    }
}