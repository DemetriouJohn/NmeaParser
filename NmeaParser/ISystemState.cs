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
        DateTime FixTime { get; }
        double MagneticVariation { get; }
        FixData CurrentFix { get; }
        RmbLine Rmb { get; }
        RmcLine Rmc { get; }
        VtgLine Vtg { get; }
        GgaLine Gga { get; }
        GSALine Gsa { get; }
    }
}