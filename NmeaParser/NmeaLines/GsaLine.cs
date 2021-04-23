using NmeaParser.NmeaLines.Enums;
using System.Collections.Generic;
using System.Globalization;

namespace NmeaParser.NmeaLines
{
    public sealed class GsaLine : NmeaMessage
    {
        public GsaLine(string nmeaLine) : base(NmeaType.Gsa, nmeaLine)
        {
            Mode = message[0] == "A" ? ModeSelection.Auto : ModeSelection.Manual;
            Fix = (FixType)int.Parse(message[1], CultureInfo.InvariantCulture);

            List<int> svs = new List<int>();
            for (int i = 2; i < 14; i++)
            {
                if (message[i].Length > 0 && int.TryParse(message[i], out var id))
                {
                    svs.Add(id);
                }
            }

            SatelliteIDs = svs.ToArray();

            double tmp;
            if (double.TryParse(message[14], NumberStyles.Float, CultureInfo.InvariantCulture, out tmp))
            {
                Pdop = tmp;
            }
            if (double.TryParse(message[15], NumberStyles.Float, CultureInfo.InvariantCulture, out tmp))
            {
                Hdop = tmp;
            }

            if (double.TryParse(message[16], NumberStyles.Float, CultureInfo.InvariantCulture, out tmp))
            {
                Vdop = tmp;
            }
        }

        public ModeSelection Mode { get; }
        public FixType Fix { get; }
        public double Pdop { get; } = double.NaN;
        public double Hdop { get; } = double.NaN;
        public double Vdop { get; } = double.NaN;
        public int[] SatelliteIDs { get; }
    }
}
