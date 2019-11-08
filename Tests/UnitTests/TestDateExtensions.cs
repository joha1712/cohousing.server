using System;
using Cohousing.Server.Util;
using Xunit;

namespace UnitTests
{
    public class TestDateExtensions
    {
        [Theory]
        [InlineData("2018-12-31 10:00:00", "2018-12-31 00:00:00")]
        [InlineData("2019-01-01 10:00:00", "2018-12-31 00:00:00")]
        [InlineData("2019-01-02 23:00:00", "2018-12-31 00:00:00")]
        [InlineData("2019-01-03 23:00:00", "2018-12-31 00:00:00")]
        [InlineData("2019-01-04 23:00:00", "2018-12-31 00:00:00")]
        [InlineData("2019-01-05 23:00:00", "2018-12-31 00:00:00")]
        [InlineData("2019-01-06 23:00:00", "2018-12-31 00:00:00")]
        public void TestStartOfWeekDate(string input, string expectedResult)
        {
            // Arrange
            var inputAsDate = DateTime.Parse(input);
            var expectedResultAsDate = DateTime.Parse(expectedResult);
            
            var result = inputAsDate.StartOfWeekDate();
            
            Assert.Equal(expectedResultAsDate, result);
        }
    }
}