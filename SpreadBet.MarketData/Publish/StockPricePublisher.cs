using SpreadBet.Infrastructure.Messaging;

namespace SpreadBet.MarketData.Publish
{
    public class StockPricePublisher
    {
        private readonly IMarket _market;
        private readonly IStockList _stockList;
        private readonly IPriceGateway _priceGateway;
        private readonly IMessageSender _channel;

        public StockPricePublisher(IMarket market, IStockList stockList, IPriceGateway priceGateway, IMessageSender channel)
        {
            this._market = market;
            this._stockList = stockList;
            this._priceGateway = priceGateway;
            this._channel = channel;
        }

        public void Publish()
        {
            if (_market.IsOpen)
            {
                foreach (var stock in _stockList.GetStocks())
                {
                    var price = _priceGateway.GetStockPrice(stock.Identifier);

                    _channel.Send(new Message());
                }
            }
        }
    }
}
