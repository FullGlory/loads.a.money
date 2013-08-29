using SpreadBet.Domain;
using SpreadBet.Infrastructure;
using SpreadBet.Infrastructure.Messaging;

namespace SpreadBet.MarketData
{
    public abstract class PriceFeedPublishProcessor : IProcessor
    {
        private readonly ISender<StockPrice> _outputChannel;

        protected PriceFeedPublishProcessor(ISender<StockPrice> outputChannel)
        {
            this._outputChannel = outputChannel;
        }

        protected ISender<StockPrice> GetPriceFeed ()
        { 
            return this._outputChannel;    
        }

        public void Start()
        {
            OnStart();
        }

        protected abstract void OnStart();

        public void Stop()
        {
            OnStop();
        }

        protected abstract void OnStop();
    }
}
