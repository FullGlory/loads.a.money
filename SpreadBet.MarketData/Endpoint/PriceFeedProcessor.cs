using SpreadBet.Domain;
using SpreadBet.Infrastructure;

namespace SpreadBet.MarketData.Endpoint
{
    public class PriceFeedProcessor : IProcessor
    {
        private readonly IReceiver<StockPrice> _receiver;
        private readonly ISender<StockPrice> _sender;

        public PriceFeedProcessor(IReceiver<StockPrice> receiver, ISender<StockPrice> sender)
        {
            this._receiver = receiver;
            this._sender = sender;
        }
        public void Start()
        {
            this._receiver.Start((stockPrice) =>
                {
                    this._sender.Send(stockPrice);
                });
        }

        public void Stop()
        {
            this._receiver.Stop();
        }
    }
}
