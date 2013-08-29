using System;
using SpreadBet.Domain;
using SpreadBet.Infrastructure.Messaging;
using SpreadBet.Scheduler;

namespace SpreadBet.MarketData
{
    public class PriceFeedBotProcessor : PriceFeedPublishProcessor
    {
        private readonly IScheduler _scheduler;
        private readonly IStockPriceBot _bot;
        private readonly string _schedulerConfig;
        private bool _schedulerConfigured = false;

        public PriceFeedBotProcessor(IScheduler scheduler, IStockPriceBot bot, ISender<StockPrice> priceFeed, string schedulerConfig)
            : base(priceFeed)
        {
            this._scheduler = scheduler;
            this._bot = bot;
            this._schedulerConfig = schedulerConfig;
        }

        protected override void OnStart()
        {
            if (!_schedulerConfigured)
            {
                this._scheduler.AddScheduledAction(
                    () => 
                    {
                        this._bot.Scrape((sp) => this.GetPriceFeed().Send(sp));
                    }, 
                    TimeSpan.Parse(this._schedulerConfig));

                _schedulerConfigured = true;
            }

            this._scheduler.Start();
        }

        protected override void OnStop()
        {
            this._bot.Stop();
            this._scheduler.Stop();
        }
    }
}
