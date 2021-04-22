using ExtendedGeoCoordinate;
using System;

namespace NmeaParser.NmeaLines
{
    public sealed class GllLine : NmeaMessage
    {
        public GllLine(string nmeaLine) : base(NmeaType.Gll, nmeaLine)
        {
            var latitude = Helper.StringToLatitude(message[0], message[1]);
            var longitude = Helper.StringToLongitude(message[2], message[3]);
            Position = new GeoCoordinate(latitude, longitude);

            if (message.Length >= 5) //Some older GPS doesn't broadcast fix time
            {
                FixTime = DateTime.Today + Helper.StringToTimeSpan(message[4]);
            }

            DataActive = message.Length < 6 || message[5] == "A";
            Mode = DataActive ? GllMode.Autonomous : GllMode.DataNotValid;
            if (message.Length > 6)
            {
                switch (message[6])
                {
                    case "A": Mode = GllMode.Autonomous; break;
                    case "D": Mode = GllMode.DataNotValid; break;
                    case "E": Mode = GllMode.EstimatedDeadReckoning; break;
                    case "M": Mode = GllMode.Manual; break;
                    case "S": Mode = GllMode.Simulator; break;
                    case "N": Mode = GllMode.DataNotValid; break;
                }
            }
        }

        public GeoCoordinate Position { get; }
        public DateTime FixTime { get; }
        public bool DataActive { get; }
        public GllMode Mode { get; }
    }
}
