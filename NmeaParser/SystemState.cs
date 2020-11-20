﻿using ExtendedGeoCoordinate;
using NmeaParser.NmeaLines;
using System;

namespace NmeaParser
{
    internal sealed class SystemState : ISystemState
    {
        /// <summary>
        /// The Latest RMB line parsed
        /// </summary>
        public RMBLine Rmb { get; private set; }

        /// <summary>
        /// The Latest RMC line parsed
        /// </summary>
        public RMCLine Rmc { get; private set; }

        /// <summary>
        /// Velocity towards destination in knots
        /// </summary>
        public double Velocity { get; private set; }

        /// <summary>
        /// Current Position
        /// </summary>
        public GeoCoordinate CurrentPosition { get; private set; }

        /// <summary>
        /// Magnetic Variation
        /// </summary>
        public double MagneticVariation { get; private set; }

        /// <summary>
        /// Fix Time
        /// </summary>
        public DateTimeOffset FixTime { get; private set; }

        /// <summary>
        /// System Speed
        /// </summary>
        public double Speed => CurrentPosition.Speed;

        /// <summary>
        /// System Course
        /// </summary>
        public double Course => CurrentPosition.Course;

        internal void Handle(RMBLine nmeaMessage)
        {
            Rmb = nmeaMessage;
            Velocity = nmeaMessage.Velocity;
        }

        internal void Handle(RMCLine nmeaMessage)
        {
            Rmc = nmeaMessage;
            CurrentPosition = nmeaMessage.GeoCoordinate;
            MagneticVariation = nmeaMessage.MagneticVariation;
            FixTime = nmeaMessage.FixTime;
        }
    }
}