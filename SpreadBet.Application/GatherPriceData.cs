using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpreadBet.Common.Interfaces;
using CuttingEdge.Conditions;
using SpreadBet.Infrastructure;
using SpreadBet.Scheduler;

namespace SpreadBet.Application
{
	/// <summary>
	///		Gathers the stock price information 
	/// </summary>
	public class GatherPriceData: IProcessor
	{
		private readonly IStockPriceProvider _priceProvider;
		private readonly IStockDataProvider _stockDataProvider;
        private readonly IScheduler _scheduler;

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

            // TODO - inject this
            this._scheduler = new Scheduler.Scheduler();

            // Schedule the scrape every minute
            this._scheduler.AddScheduledAction(()=>GetStockPrices(), new TimeSpan(0, 1, 0));
		}

        private void GetStockPrices()
        {
            var stockPrices = this._priceProvider.GetStockPrices();
            this._stockDataProvider.SaveStockData(stockPrices);
        }

		public void Start()
		{
            this._scheduler.Start();
		}
        
        public void Stop()
        {
            this._scheduler.Stop();
        }
    }
}
