using SpreadBet.Common.Interfaces;
using SpreadBet.Domain;
using SpreadBet.Domain.Interfaces;
using SpreadBet.Infrastructure;
using SpreadBet.Infrastructure.Messaging;

namespace SpreadBet.MarketData
{
    public class PriceFeedSubscribeProcessor : IProcessor
    {
        private readonly IReceiver<StockPrice> _priceFeed;
        private readonly IStockPriceRepository _stockPriceRepository;

        public PriceFeedSubscribeProcessor(IReceiver<StockPrice> priceFeed, IStockPriceRepository stockPriceRepository)
        {
            this._priceFeed = priceFeed;
            this._stockPriceRepository = stockPriceRepository;
        }

        public void Start()
        {
            this._priceFeed.Start((sp) =>
                {
                    this._stockPriceRepository.AddStockPrice(sp);
                });  
        }

        public void Stop()
        {
            this._priceFeed.Stop();
        }
    }
}
