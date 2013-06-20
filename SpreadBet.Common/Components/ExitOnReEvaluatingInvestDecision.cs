// -----------------------------------------------------------------------
// <copyright file="ExitWhenPositionMovesAgainstBet.cs" company="">
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
	using SpreadBet.Domain;

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public class ExitOnReEvaluatingInvestDecision: IExitDecider
	{
		private readonly IInvestDecider _investDecider;

		/// <summary>
		/// Initializes a new instance of the <see cref="ExitWhenPositionMovesAgainstBet"/> class.
		/// </summary>
		/// <param name="stockHistoryDataProvider">The stock history data provider.</param>
		/// <param name="periods">The periods.</param>
		public ExitOnReEvaluatingInvestDecision(IInvestDecider investDecider)
		{
			Condition.Requires(investDecider).IsNotNull();
			this._investDecider = investDecider;
		}

		/// <summary>
		/// Gets the exit descisions.
		/// </summary>
		/// <param name="bets">The bets.</param>
		/// <returns></returns>
		public IEnumerable<Domain.Bet> GetExitDescisions(IEnumerable<Domain.Bet> bets)
		{
			var stocks = bets.Select(bet => bet.Stock).ToList();
			var investDecision = this._investDecider.GetInvestmentDescisions(stocks).ToList();

			var movingAgainst = (from currentBet in bets
								 join proposedBet in investDecision	on currentBet.Stock.Identifier equals proposedBet.Stock.Identifier
								 where currentBet.Direction != proposedBet.Direction && 
								 currentBet.IsInProfit(proposedBet.Price)
								 select currentBet).ToList();

			return movingAgainst;
		}
	}

	public static class BetExtension
	{
		public static bool IsInProfit(this Bet bet, Price currentPrice)
		{
			return (((bet.Direction == Direction.Increase) && (currentPrice.Offer >= bet.OpeningPosition)) ||
				    ((bet.Direction == Direction.Decrease) && (currentPrice.Bid <= bet.OpeningPosition)));
		}
	}
}
