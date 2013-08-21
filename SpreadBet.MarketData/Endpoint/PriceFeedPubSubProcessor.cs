using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpreadBet.Infrastructure;

namespace SpreadBet.MarketData.Endpoint
{
    public class PriceFeedPubSubProcessor : IProcessor
    {
        private readonly IProcessor _publisher;
        private readonly IProcessor _subscriber;

        public PriceFeedPubSubProcessor(IProcessor publisher, IProcessor subscriber)
        {
            this._publisher = publisher;
            this._subscriber = subscriber;
        }

        public void Start()
        {
            this._subscriber.Start();
            this._publisher.Start();
        }

        public void Stop()
        {
            this._subscriber.Stop();
            this._publisher.Stop();
        }
    }
}
