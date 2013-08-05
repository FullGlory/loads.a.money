﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SpreadBet.Common.Helpers;

namespace SpreadBet.UnitTests.Common
{
	[TestFixture]
	public class TimePeriodHelperTests
	{
		[Test]
		public void GetTimePeriod_WithKnownDateTime_ReturnsCorrectPeriodId()
		{
			var result1 = TimePeriodHelper.GetTimePeriod(DateTime.Parse("01 January 2013 03:14:23"), (60 * 60));
			var result2 = TimePeriodHelper.GetTimePeriod(DateTime.Parse("01 January 2013 04:44:23"), (60 * 60));

			Assert.AreEqual(1, result2.Id - result1.Id);
		}
	}
}