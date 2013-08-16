using SpreadBet.Common.Interfaces;
using SpreadBet.Domain;
using SpreadBet.Infrastructure.Messaging;
using System;

namespace SpreadBet.MarketData.Subscribe
{
    public class StockPriceSubscriber
    {
        private readonly IMessageReceiver _dataFeedChannel;
        private readonly IStockDataProvider _stockDataProvider;
        private readonly IMessageSender _deadLetterChannel;

        public StockPriceSubscriber(IMessageReceiver dataFeedChannel, IStockDataProvider stockDataProvider, IMessageSender deadLetterChannel)
        {
          this._dataFeedChannel = dataFeedChannel;
          this._stockDataProvider = stockDataProvider;
          this._deadLetterChannel = deadLetterChannel;

          this._dataFeedChannel.MessageReceived += MessageReceived;
        }

        public void Start()
        {
            this._dataFeedChannel.Start();
        }

        private void MessageReceived(object sender, MessageReceivedEventArgs e)
        {
          var msg = e.Message;

          try
          {
            _stockDataProvider.AddStockPrice(new StockPrice());
          }
          catch (Exception ex)
          {
            _deadLetterChannel.Send(msg);
          }
        }
    }
}
