using ExtendedGeoCoordinate;
using NmeaParser.NmeaLines;
using System;

namespace NmeaParser
{
    public interface ISystemState
    {
        double Course { get; }
        GeoCoordinate CurrentPosition { get; }
        DateTimeOffset FixTime { get; }
        double MagneticVariation { get; }
        RMBLine Rmb { get; }
        RMCLine Rmc { get; }
        double Speed { get; }
        double VelocityInKnots { get; }
        VTGLine Vtg { get; }
    }
}