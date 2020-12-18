using Xunit;
using NmeaParser.NmeaLines;
using System;
using SmartExtensions;

namespace NmeaParser.Tests
{
    public class NmeaLineFactoryTests
    {
        [Theory]
        [InlineData("$GPGGA,123519,4807.038,N,01131.000,E,1,08,0.9,545.4,M,46.9,M,,*47")]
        [InlineData("$GPGSA,A,3,04,05,,09,12,,,24,,,,,2.5,1.3,2.1*39")]
        [InlineData("$GPGSV,2,1,08,01,40,083,46,02,17,308,41,12,07,344,39,14,22,228,45*75")]
        public void ValidateChecksum_ValidLines_True(string nmeaLine)
        {
            Assert.True(new NmeaLineFactory().ValidateChecksum(nmeaLine));
        }

        [Theory]
        [InlineData("$GPGGA,123519,4807.038,N,01131.000,E,1,08,0.9,545.4,M,46.9,M,,*17")]
        [InlineData("$GPGSA,A,3,04,05,,09,12,,,24,,,,,2.5,1.3,2.1*29")]
        [InlineData("$GPGSV,2,1,08,01,40,083,46,02,17,308,41,12,07,344,39,14,22,228,45*45")]
        public void ValidateChecksum_InValidLines_False(string nmeaLine)
        {
            Assert.False(new NmeaLineFactory().ValidateChecksum(nmeaLine));
        }

        [Fact]
        public void ParseRmbLine()
        {
            const string line = "$GPRMB,A,0.66,L,003,004,4917.24,N,12309.57,W,001.3,052.5,000.5,V*20";

            var nmeaMsg = new NmeaLineFactory().ParseLine(line);
            Assert.Equal(NmeaType.Rmb, nmeaMsg.NmeaType);
            var rmb = (RmbLine)nmeaMsg;
            Assert.Equal(RmbDataStatus.Ok, rmb.Status);
            Assert.Equal(-0.66, rmb.CrossTrackError);
            Assert.Equal(3, rmb.OriginWaypointId);
            Assert.Equal(4, rmb.DestinationWaypointId);
            Assert.Equal(49.287333, rmb.DestinationGeoCoordinate.Latitude, 6);
            Assert.Equal(-123.1595, rmb.DestinationGeoCoordinate.Longitude);
            Assert.Equal(1.3, rmb.RangeToDestination);
            Assert.Equal(52.5, rmb.TrueBearing);
            Assert.False(rmb.Arrived);
        }

        [Fact]
        public void ParseRmcLine()
        {
            const string line = "$GPRMC,081836,A,3751.65,S,14507.36,E,000.0,360.0,130917,011.3,E*65";
            {
                var nmeaMsg = new NmeaLineFactory().ParseLine(line);
                Assert.Equal(NmeaType.Rmc, nmeaMsg.NmeaType);
                var rmc = (RmcLine)nmeaMsg;
                Assert.Equal(new DateTime(2017, 9, 13, 8, 18, 36), rmc.FixTime);
                Assert.Equal(-37.8608333333, rmc.GeoCoordinate.Latitude, 10);
                Assert.Equal(145.1226666667, rmc.GeoCoordinate.Longitude, 10);
                Assert.True(rmc.Active);
                Assert.Equal(360, rmc.GeoCoordinate.Course);
                Assert.Equal(11.3, rmc.MagneticVariation);
                Assert.Equal(0, rmc.GeoCoordinate.Speed);
            }
        }

        [Fact]
        public void ParseGgaLine()
        {
            const string line = "$GPGGA,115739.00,4158.8441367,N,09147.4416929,W,4,13,0.9,255.747,M,-32.00,M,01,0000*6E";

            var nmeaMsg = new NmeaLineFactory().ParseLine(line);
            Assert.Equal(NmeaType.Gga, nmeaMsg.NmeaType);
            var gga = (GgaLine)nmeaMsg;

            Assert.Equal(Helper.StringToTimeSpan("115739"), gga.FixTime);
            Assert.Equal(41.9807356117, gga.Position.Latitude, 10);
            Assert.Equal(-91.7906948817, gga.Position.Longitude, 10);
            Assert.Equal(FixQuality.RealTimeKinematic, gga.Quality);
            Assert.Equal(13, gga.NumberOfSatellites);
            Assert.Equal(0.9, gga.Hdop);
            Assert.Equal(255.747, gga.Position.Altitude);
            Assert.Equal("M", gga.Position.AltitudeUnits);
            Assert.Equal(-32, gga.GeoidalSeparation);
            Assert.Equal("M", gga.GeoidalSeparationUnits);
            Assert.Equal(1.Seconds(), gga.TimeSinceLastDgpsUpdate);
            Assert.Equal(0, gga.DgpsStationId);
        }
    }
}
