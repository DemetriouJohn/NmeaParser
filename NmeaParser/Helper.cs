using System.Globalization;

namespace NmeaParser
{
    internal class Helper
    {
        internal static double StringToLatitude(string value, string ns)
        {
            if (value == null || value.Length < 3)
            {
                return double.NaN;
            }

            double latitude = int.Parse(value.Substring(0, 2), CultureInfo.InvariantCulture)
               + double.Parse(value.Substring(2), CultureInfo.InvariantCulture) / 60;

            if (ns == "S")
            {
                latitude *= -1;
            }

            return latitude;
        }

        internal static double StringToLongitude(string value, string ew)
        {
            if (value == null || value.Length < 4)
            {
                return double.NaN;
            }

            double longitude = int.Parse(value.Substring(0, 3), CultureInfo.InvariantCulture)
               + double.Parse(value.Substring(3), CultureInfo.InvariantCulture) / 60;
            if (ew == "W")
            {
                longitude *= -1;
            }

            return longitude;
        }
    }
}