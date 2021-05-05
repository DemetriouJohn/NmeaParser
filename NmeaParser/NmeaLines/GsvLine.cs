using System;
using System.Collections.Generic;

namespace NmeaParser.NmeaLines
{
    public class GsvLine : NmeaMessage
    {
        public GsvLine(string nmeaLine) : base(NmeaType.Gsv, nmeaLine)
        {
            if (message.Length < 6) throw new ArgumentOutOfRangeException(nameof(nmeaLine), $"Not enough fields in message: {message.Length} but expecting at least 6.");
            if (message.Length % 4 != 3) throw new ArgumentOutOfRangeException(nameof(nmeaLine), $"Invalid number of fields: {message.Length} but expecting 3 + an integer multiple of 4.");

            MessageCount = int.TryParse(message[0], out int messageCount) ? messageCount : throw new ArgumentOutOfRangeException(nameof(nmeaLine), $"Invalid value for field 0: {message[0]}");
            MessageIndex = int.TryParse(message[1], out int messageIndex) ? messageIndex : throw new ArgumentOutOfRangeException(nameof(nmeaLine), $"Invalid value for field 1: {message[1]}");
            SatelliteCount = int.TryParse(message[2], out int satelliteCount) ? satelliteCount : throw new ArgumentOutOfRangeException(nameof(nmeaLine), $"Invalid value for field 2: {message[2]}");

            int i = 3;
            var satellites = new List<FixSatellite>();
            while (i < message.Length)
            {
                if (!int.TryParse(message[i++], out int id)) throw new ArgumentOutOfRangeException(nameof(nmeaLine), $"Invalid value at field {i - i}: {message[i - 1]}");
                if (!int.TryParse(message[i++], out int elevation)) throw new ArgumentOutOfRangeException(nameof(nmeaLine), $"Invalid value for field {i - 1}: {message[i - 1]}");
                if (!int.TryParse(message[i++], out int azimuth)) throw new ArgumentOutOfRangeException(nameof(nmeaLine), $"Invalid value for field {i - 1}: {message[i - 1]}");
                if (!int.TryParse(message[i++], out int snr)) throw new ArgumentOutOfRangeException(nameof(nmeaLine), $"Invalid value for field {i - 1}: {message[i - 1]}");
                satellites.Add(new FixSatellite(id, elevation, azimuth, snr));
            }
            Satellites = satellites.ToArray();
        }

        /// <summary>
        /// Number of messages needed for full data
        /// </summary>
        public int MessageCount { get; }

        /// <summary>
        /// Message index in group
        /// </summary>
        public int MessageIndex { get; }

        /// <summary>
        /// Number of satellites in view
        /// </summary>
        public int SatelliteCount { get; }

        /// <summary>
        /// Satellite data in this message
        /// </summary>
        public FixSatellite[] Satellites { get; }
    }
}
