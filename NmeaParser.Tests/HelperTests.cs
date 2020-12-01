using System;
using Xunit;

namespace NmeaParser.Tests
{
    public class HelperTests
    {
        [Fact]
        public void StringToLatitudeValidValueNorth()
        {
            var lat = "4917.24";
            var orientation = "N";

            Assert.Equal(49.287333, Helper.StringToLatitude(lat, orientation), 6);
        }

        [Fact]
        public void StringToLatitudeValidValueSouth()
        {
            var lat = "4917.24";
            var orientation = "S";

            Assert.Equal(-49.287333, Helper.StringToLatitude(lat, orientation), 6);
        }

        [Fact]
        public void StringToLongitudeValidValueWest()
        {
            var lat = "12309.57";
            var orientation = "W";

            Assert.Equal(-123.1595, Helper.StringToLongitude(lat, orientation), 6);
        }

        [Fact]
        public void StringToLongitudeValidValueEast()
        {
            var lat = "12309.57";
            var orientation = "E";

            Assert.Equal(123.1595, Helper.StringToLongitude(lat, orientation), 6);
        }

        [Fact]
        public void StringToTimeStampTest()
        {
            Assert.Equal(new TimeSpan(11,57,39), Helper.StringToTimeSpan("115739"));
        }
    }
}
