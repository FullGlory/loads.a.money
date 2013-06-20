// -----------------------------------------------------------------------
// <copyright file="ExitAfterInvestmentPeriod.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SpreadBet.Common.Components
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using SpreadBet.Common.Interfaces;
	using CuttingEdge.Conditions;

	/// <summary>
	/// Arrives at an exit position based purely on the length of time we have been betting on a particular stock
	/// </summary>
	public class ExitAfterInvestmentPeriod : IExitDecider
	{
		private readonly int _periodHours;

		public ExitAfterInvestmentPeriod(int periodHours)
		{
			Condition.Requires(periodHours).IsGreaterOrEqual(1);

			this._periodHours = periodHours;
		}

		public IEnumerable<Domain.Bet> GetExitDescisions(IEnumerable<Domain.Bet> bets)
		{
			var retVal = new List<Domain.Bet>();

			var now = DateTime.UtcNow;

			bets.ToList().ForEach(bet =>
			{
				if ((now - (DateTime)bet.PlacedOn).TotalHours >= this._periodHours)
					retVal.Add(bet);
			});

			return retVal;
		}
	}
}
