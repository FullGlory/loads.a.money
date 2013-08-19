using SpreadBet.Domain;
using SpreadBet.Infrastructure.Messaging;
using SpreadBet.Infrastructure.Serialisation;

namespace SpreadBet.MarketData.Publish
{
    public class StockPricePublisher
    {
        private readonly IMarket _market;
        private readonly IStockList _stockList;
        private readonly IPriceGateway _priceGateway;
        private readonly ITextSerialiser _serialiser;
        private readonly IMessageSender _channel;

        public StockPricePublisher(IMarket market, IStockList stockList, IPriceGateway priceGateway, ITextSerialiser serialiser, IMessageSender channel)
        {
            this._market = market;
            this._stockList = stockList;
            this._priceGateway = priceGateway;
            this._serialiser = serialiser;
            this._channel = channel;
        }

        public void Publish()
        {
            if (_market.IsOpen)
            {
                foreach (var stock in _stockList.GetStocks())
                {
                    var price = _priceGateway.GetStockPrice(stock.Identifier);

                    // TODO - set period
                    var stockPrice = new StockPrice
                    {
                      Price = price,
                      Stock = stock
                    };

                    _channel.Send(new Message{Body=_serialiser.Serialize(stockPrice)});
                }
            }
        }
    }
}
