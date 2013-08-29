using System;
using SpreadBet.Domain;
using SpreadBet.Scheduler;

namespace SpreadBet.MarketData
{
    public class PriceFeedBotProcessor : PriceFeedPublishProcessor
    {
        private readonly IScheduler _scheduler;
        private readonly IStockPriceBot _bot;

        public PriceFeedBotProcessor(IScheduler scheduler, IStockPriceBot bot, ISender<StockPrice> priceFeed)
            : base(priceFeed)
        {
            this._scheduler = scheduler;
            this._bot = bot;
            this._scheduler.AddScheduledAction(Scheduler_Elapsed, TimeSpan.FromSeconds(5));
        }

        protected override void OnStart()
        {
            this._scheduler.Start();
        }

        private void Scheduler_Elapsed()
        {
            this._scheduler.Stop();
            this._bot.Scrape((sp) => this.GetPriceFeed().Send(sp));
            this._scheduler.Start();
        }

        protected override void OnStop()
        {
            this._bot.Stop();
            this._scheduler.Stop();
        }
    }
}
