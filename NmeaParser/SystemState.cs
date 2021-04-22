using ExtendedGeoCoordinate;
using NmeaParser.NmeaLines;
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
        /// The Latest GSA line parsed
        /// </summary>
        public GSALine Gsa { get; private set; }
        
        /// <summary>
        /// The Latest GLL line parsed
        /// </summary>
        public GllLine Gll { get; private set; }

        /// <summary>
        /// Velocity towards destination in knots
        /// </summary>
        public double VelocityInKnots { get; private set; }

        /// <summary>
        /// Current Position
        /// </summary>
        public GeoCoordinate CurrentPosition { get; private set; }

        /// <summary>
        /// Current GLL mode
        /// </summary>
        public GllMode Mode { get; private set; }

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

        /// <summary>
        /// System Course
        /// </summary>
        public double Course => CurrentPosition.Course;

        /// <summary>
        /// System current fix data
        /// </summary>
        public FixData CurrentFix { get; private set; }

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

            Mode = nmeaMessage.Mode;
            FixTime = nmeaMessage.FixTime;
        }

        internal void Handle(GSALine nmeaMessage)
        {
            Gsa = nmeaMessage;
            CurrentFix = new FixData(
                nmeaMessage.IsAuto,
                nmeaMessage.Status,
                nmeaMessage.PDOP,
                nmeaMessage.HDOP,
                nmeaMessage.VDOP,
                nmeaMessage.SatelliteIds);
        }
    }
}