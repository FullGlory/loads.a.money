// -----------------------------------------------------------------------
// <copyright file="GetBetDecisions.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SpreadBet.Application
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using SpreadBet.Common.Interfaces;
	using CuttingEdge.Conditions;

	/// <summary>
	/// Returns a list of bet decisions
	/// </summary>
	public class GetBetDecisions: IExecutableApplication
	{
		private readonly IInvestDecider _investDecider;
		private readonly IStockFilter _stockFilter;
		private readonly IStockDataProvider _stockProvider;

		/// <summary>
		/// Initializes a new instance of the <see cref="GatherPriceData"/> class.
		/// </summary>
		/// <param name="stockPriceProvider">The stock price provider.</param>
		public GetBetDecisions(IStockDataProvider stockProvider, 
							   IInvestDecider investDecider, 
							   IStockFilter stockFilter)
		{
			Condition.Requires(stockProvider).IsNotNull();
			Condition.Requires(investDecider).IsNotNull();
			Condition.Requires(stockFilter).IsNotNull();

			this._stockFilter = stockFilter;
			this._investDecider = investDecider;
			this._stockProvider = stockProvider;
		}

		public void Run()
		{
			// Get all the stocks we know about
			var stocks = this._stockProvider.GetStocks();

			// Determine which ones look interesting
			var candidates = this._stockFilter.GetInvestmentCandidates(stocks);

			// Determine what the bets might look like
			var bets = this._investDecider.GetInvestmentDescisions(candidates);

			foreach (var bet in bets)
			{
				Console.WriteLine("Stock:{0} BidAmount:{1} ExitPrice{2} Direction:{3}", bet.Stock.Name, bet.BidAmount, bet.ExitPrice, bet.Direction.ToString());
			}
		}
	}
}
