using System;
using SpreadBet.Domain;

namespace SpreadBet.MarketData
{
    public interface IStockPriceBot
    {
        void Scrape(Action<StockPrice> onPriceRead);

        void Stop();
    }
}
