// -----------------------------------------------------------------------
// <copyright file="TimePeriodHelper.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SpreadBet.Common.Helpers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using SpreadBet.Domain;

	public class TimePeriodHelper
	{
		private readonly static long __PERIODSEED;

		static TimePeriodHelper()
		{
			__PERIODSEED = DateTime.Parse("01 January 2013").Ticks;
		}

		public static Period GetTimePeriod(DateTime dateTime, int periodLengthSecs = 3600)
		{
			var ticks = dateTime.Ticks - __PERIODSEED;
			var ticksPerPeriod = new TimeSpan(0, 0, periodLengthSecs).Ticks;

			var periodStart = Math.Floor((decimal)ticks / ticksPerPeriod) + 1;

			return new Period
			{
				Id = (int)periodStart,
				From = new DateTime((int)periodStart + __PERIODSEED),
				To = new DateTime((int)periodStart + __PERIODSEED).AddSeconds(periodLengthSecs)
			};
		}
	}
}
