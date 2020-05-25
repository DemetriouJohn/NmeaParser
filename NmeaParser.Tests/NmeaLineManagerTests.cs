using System;
using Xunit;
using NmeaParser;

namespace NmeaParser.Tests
{
    public class NmeaLineManagerTests
    {
        [Theory]
        [InlineData("$GPGGA,123519,4807.038,N,01131.000,E,1,08,0.9,545.4,M,46.9,M,,*47")]
        [InlineData("$GPGSA,A,3,04,05,,09,12,,,24,,,,,2.5,1.3,2.1*39")]
        [InlineData("$GPGSV,2,1,08,01,40,083,46,02,17,308,41,12,07,344,39,14,22,228,45*75")]
        public void ValidateChecksum_ValidLines_True(string nmeaLine)
        {
            Assert.True(new NmeaLineManager().ValidateChecksum(nmeaLine));
        }

         [Theory]
        [InlineData("$GPGGA,123519,4807.038,N,01131.000,E,1,08,0.9,545.4,M,46.9,M,,*17")]
        [InlineData("$GPGSA,A,3,04,05,,09,12,,,24,,,,,2.5,1.3,2.1*29")]
        [InlineData("$GPGSV,2,1,08,01,40,083,46,02,17,308,41,12,07,344,39,14,22,228,45*45")]
        public void ValidateChecksum_InValidLines_False(string nmeaLine)
        {
            Assert.False(new NmeaLineManager().ValidateChecksum(nmeaLine));
        }
    }
}
