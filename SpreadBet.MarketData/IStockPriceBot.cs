using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpreadBet.Domain;

namespace SpreadBet.MarketData
{
    public interface IStockPriceBot
    {
        void Scrape(Action<StockPrice> onPriceRead);
    }
}
