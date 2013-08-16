using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpreadBet.Common.Interfaces;
using SpreadBet.Domain;
using SpreadBet.Infrastructure.Messaging;

namespace SpreadBet.MarketData.Subscribe
{
    public class StockPriceSubscriber
    {
        private readonly IMessageReceiver _channel;
        private readonly IStockDataProvider _stockDataProvider;

        public StockPriceSubscriber(IMessageReceiver channel, IStockDataProvider stockDataProvider)
        {
            this._channel = channel;
            this._stockDataProvider = stockDataProvider;

            this._channel.MessageReceived += MessageReceived;
        }

        public void Start()
        {
            this._channel.Start();
        }

        private void MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            _stockDataProvider.AddStockPrice(new StockPrice());
        }
    }
}
