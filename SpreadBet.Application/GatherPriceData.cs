using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpreadBet.Common.Interfaces;
using CuttingEdge.Conditions;
using SpreadBet.Repository;
using SpreadBet.Infrastructure;

namespace SpreadBet.Application
{
	/// <summary>
	///		Gathers the stock price information 
	/// </summary>
	public class GatherPriceData: IProcessor
	{
		private readonly IStockPriceProvider _priceProvider;
		private readonly IStockDataProvider _stockDataProvider;

		/// <summary>
		/// Initializes a new instance of the <see cref="GatherPriceData"/> class.
		/// </summary>
		/// <param name="stockPriceProvider">The stock price provider.</param>
		public GatherPriceData(IStockPriceProvider stockPriceProvider, 
							   IStockDataProvider stockDataProvider)
		{
			Condition.Requires(stockPriceProvider).IsNotNull();
			Condition.Requires(stockDataProvider).IsNotNull();

			this._priceProvider = stockPriceProvider;
			this._stockDataProvider = stockDataProvider;
		}

		public void Start()
		{
			var stockPrices = this._priceProvider.GetStockPrices();
			this._stockDataProvider.SaveStockData(stockPrices);
		}
        
        public void Stop()
        {
            // TODO - Do nothing
        }
    }
}
