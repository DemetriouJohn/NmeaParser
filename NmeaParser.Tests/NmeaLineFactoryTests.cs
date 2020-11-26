using Xunit;
using NmeaParser.NmeaLines;
using System;

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
            var rmb = (RMBLine)nmeaMsg;
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
            const string line = "$GNRMC,231011.00,A,3403.47163804,N,11711.80926595,W,0.019,11.218,201217,12.0187,E,D*01";

            var nmeaMsg = new NmeaLineFactory().ParseLine(line);
            Assert.Equal(NmeaType.Rmc, nmeaMsg.NmeaType);
            var rmc = (RMCLine)nmeaMsg;
            Assert.Equal(new DateTimeOffset(2017, 12, 20, 23, 10, 11, TimeSpan.Zero), rmc.FixTime);
            Assert.Equal(34.057860634, rmc.GeoCoordinate.Latitude, 10);
            Assert.Equal(-117.19682109916667, rmc.GeoCoordinate.Longitude, 10);
            Assert.True(rmc.Active);
            Assert.Equal(11.218, rmc.GeoCoordinate.Course);
            Assert.Equal(12.0187, rmc.MagneticVariation);
            Assert.Equal(0.019, rmc.GeoCoordinate.Speed);
        }
    }
}