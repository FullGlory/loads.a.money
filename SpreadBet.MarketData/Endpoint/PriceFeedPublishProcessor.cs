using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpreadBet.Common.Interfaces;
using SpreadBet.Domain;
using SpreadBet.Infrastructure;

namespace SpreadBet.MarketData.Endpoint
{
    public class PriceFeedPublishProcessor : IProcessor
    {
        private readonly ISender<StockPrice> _outputChannel;

        public PriceFeedPublishProcessor(ISender<StockPrice> outputChannel)
        {
            this._outputChannel = outputChannel;
        }

        public void Start()
        {
            this._outputChannel.Send(new StockPrice());
        }

        public void Stop()
        {
            
        }
    }
}
