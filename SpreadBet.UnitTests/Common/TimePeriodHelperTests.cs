using System;
using NUnit.Framework;
using SpreadBet.Common.Helpers;

namespace SpreadBet.UnitTests.Common
{
	[TestFixture]
	public class TimePeriodHelperTests
	{
        [TestCase("01 January 2013 03:14:23", "01 January 2013 04:14:23", 1)]
        [TestCase("01 January 2013 03:14:23", "01 January 2013 04:34:23", 1)]
        [TestCase("01 January 2013 03:14:23", "01 January 2013 05:14:23", 2)]
		public void GetTimePeriod_WithKnownDateTime_ReturnsCorrectPeriodId(string period1, string period2, int expectedPeriodIdDiff)
		{
            var result1 = TimePeriodHelper.GetTimePeriod(DateTime.Parse(period1), (60 * 60));
            var result2 = TimePeriodHelper.GetTimePeriod(DateTime.Parse(period2), (60 * 60));

            Assert.AreEqual(expectedPeriodIdDiff, result2.PeriodId - result1.PeriodId);
		}

        [Test]
        public void GetTimePeriod_WithKnownDateTime_ReturnsCorrectDates()
        {
            var result1 = TimePeriodHelper.GetTimePeriod(DateTime.Parse("09 June 2013 03:14:23"));

            Assert.AreEqual(9, result1.From.Day, "From Day");
            Assert.AreEqual(6, result1.From.Month, "From Month");
            Assert.AreEqual(2013, result1.From.Year, "From Month");

            Assert.AreEqual(9, result1.To.Day, "To Day");
            Assert.AreEqual(6, result1.To.Month, "To Month");
            Assert.AreEqual(2013, result1.To.Year, "To Month");
        }
	}
}
