using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpreadBet.Common.Interfaces;
using SpreadBet.Domain;
using SpreadBet.Scheduler;

namespace SpreadBet.MarketData.Publish
{
    public class LiveChartsStockPriceReceiver : IReceiver<StockPrice>
    {
        private bool _schedulerInitialised;
        private readonly IScheduler _scheduler;
        private readonly IStockMarket _stockMarket;
        private readonly IScraper _scraper;

        public LiveChartsStockPriceReceiver(IScheduler scheduler, IStockMarket stockMarket, IScraper scraper)
        {
            this._scheduler = scheduler;
            this._stockMarket = stockMarket;
            this._scraper = scraper;
        }

        public void Start(Action<StockPrice> onReceive)
        {
            if (!_schedulerInitialised)
            {
                this._scheduler.AddScheduledAction(Scheduler_Elapsed, TimeSpan.FromSeconds(30));
                this._schedulerInitialised = true;
            }

            this._scheduler.Start();
        }

        public void Stop()
        {
            this._scheduler.Stop();
        }

        private void Scheduler_Elapsed()
        {

        }
    }
}
