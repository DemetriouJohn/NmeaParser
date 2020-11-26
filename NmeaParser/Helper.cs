using System;
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

        internal static TimeSpan StringToTimeSpan(string value)
        {
            if (value != null && value.Length >= 6)
            {
                return new TimeSpan(int.Parse(value.Substring(0, 2), CultureInfo.InvariantCulture),
                                   int.Parse(value.Substring(2, 2), CultureInfo.InvariantCulture), 0)
                                   .Add(TimeSpan.FromSeconds(double.Parse(value.Substring(4), CultureInfo.InvariantCulture)));
            }
            return TimeSpan.Zero;
        }
    }
}