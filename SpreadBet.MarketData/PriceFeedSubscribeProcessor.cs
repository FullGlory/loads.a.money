using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpreadBet.Common.Interfaces;
using SpreadBet.Domain;
using SpreadBet.Infrastructure;

namespace SpreadBet.MarketData
{
    public class PriceFeedSubscribeProcessor : IProcessor
    {
        private readonly IReceiver<StockPrice> _priceFeed;
        private readonly IStockDataProvider _stockDataProvider;

        public PriceFeedSubscribeProcessor(IReceiver<StockPrice> priceFeed, IStockDataProvider stockDataProvider)
        {
            this._priceFeed = priceFeed;
            this._stockDataProvider = stockDataProvider;
        }

        public void Start()
        {
            this._priceFeed.Start((sp) =>
                {
                    this._stockDataProvider.AddStockPrice(sp);
                });  
        }

        public void Stop()
        {
            this._priceFeed.Stop();
        }
    }
}
