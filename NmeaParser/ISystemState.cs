using ExtendedGeoCoordinate;
using NmeaParser.NmeaLines;
using NmeaParser.NmeaLines.Enums;
using System;

namespace NmeaParser
{
    public interface ISystemState
    {
        double Course { get; }
        GeoCoordinate CurrentPosition { get; }
        DateTime FixTime { get; }
        FixType FixType { get; }
        GgaLine Gga { get; }
        GllLine Gll { get; }
        GllMode GllMode { get; }
        GsaLine Gsa { get; }
        ModeSelection GsaMode { get; }
        double HDop { get; }
        double MagneticVariation { get; }
        double PDop { get; }
        RmbLine Rmb { get; }
        RmcLine Rmc { get; }
        int[] SatelliteIds { get; }
        double Speed { get; }
        double VDop { get; }
        double VelocityInKnots { get; }
        VtgLine Vtg { get; }
    }
}