using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpreadBet.Common.Interfaces;
using SpreadBet.Domain;
using SpreadBet.Infrastructure;

namespace SpreadBet.MarketData.Endpoint
{
    public class PriceFeedSubscribeProcessor : IProcessor
    {
        private readonly IReceiver<StockPrice> _inputChannel;
        private readonly IStockDataProvider _stockDataProvider;

        public PriceFeedSubscribeProcessor(IReceiver<StockPrice> inputChannel, IStockDataProvider stockDataProvider)
        {
            this._inputChannel = inputChannel;
            this._stockDataProvider = stockDataProvider;
        }

        public void Start()
        {
            this._inputChannel.Start((sp)=>this._stockDataProvider.AddStockPrice(sp));  
        }

        public void Stop()
        {
            this._inputChannel.Stop();
        }
    }
}
