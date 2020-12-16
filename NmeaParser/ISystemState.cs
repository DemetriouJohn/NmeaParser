using ExtendedGeoCoordinate;
using NmeaParser.NmeaLines;
using System;

namespace NmeaParser
{
    public interface ISystemState
    {
        double Course { get; }
        double Speed { get; }
        double VelocityInKnots { get; }
        GeoCoordinate CurrentPosition { get; }
        DateTimeOffset FixTime { get; }
        double MagneticVariation { get; }
        RmbLine Rmb { get; }
        RmcLine Rmc { get; }
        VtgLine Vtg { get; }
        GgaLine Gga { get; }
    }
}