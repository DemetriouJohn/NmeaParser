using System;
using System.Collections.Generic;

namespace NmeaParser.NmeaLines
{
    public sealed class GSALine : NmeaMessage
    {
        public GSALine(string nmeaLine) : base(NmeaType.Gsa)
        {
            var message = nmeaLine.Split(new[] { ',' }, StringSplitOptions.None);
            if (message.Length < 17) throw new ArgumentOutOfRangeException(nameof(nmeaLine), $"Not enough fields in message: {message.Length} but expecting at least 17.");

            switch (message[0])
            {
                case "A": IsAuto = true; break;
                case "M": IsAuto = false; break;
                default: throw new ArgumentOutOfRangeException(nameof(nmeaLine), $"Invalid value at field 0: {message[0]}");
            }
            switch (message[1])
            {
                case "1": Status = FixStatus.None; break;
                case "2": Status = FixStatus.Fixed2D; break;
                case "3": Status = FixStatus.Fixed3D; break;
                default: throw new ArgumentOutOfRangeException(nameof(nmeaLine), $"Invalid value at field 1: {message[1]}");
            }
            var satelliteIds = new List<string>(12);
            for (int i = 2; i < 14; i++)
            {
                if (!string.IsNullOrWhiteSpace(message[i])) satelliteIds.Add(message[i]);
            }
            SatelliteIds = satelliteIds.ToArray();
            PDOP = double.TryParse(message[14], out double pdop) ? pdop : throw new ArgumentOutOfRangeException(nameof(nmeaLine), $"Invalid value at field 14: {message[14]}");
            HDOP = double.TryParse(message[15], out double hdop) ? hdop : throw new ArgumentOutOfRangeException(nameof(nmeaLine), $"Invalid value at field 15: {message[15]}");
            VDOP = double.TryParse(message[16], out double vdop) ? vdop : throw new ArgumentOutOfRangeException(nameof(nmeaLine), $"Invalid value at field 16: {message[16]}");
        }

        /// <summary>
        /// Fix selection mode
        /// </summary>
        public bool IsAuto { get; }

        /// <summary>
        /// Fix mode
        /// </summary>
        public FixStatus Status { get; }

        /// <summary>
        /// Satellite IDs included in the fix
        /// </summary>
        public string[] SatelliteIds { get; }

        /// <summary>
        /// Dilution of position
        /// </summary>
        public double PDOP { get; }

        /// <summary>
        /// Horizontal dilution of precision
        /// </summary>
        public double HDOP { get; }

        /// <summary>
        /// Vertical dilution of position
        /// </summary>
        public double VDOP { get; }
    }
}
