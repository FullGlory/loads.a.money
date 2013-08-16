using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpreadBet.Domain;

namespace SpreadBet.MarketData.Publish
{
    public interface IPriceGateway
    {
        Price GetStockPrice(string identifier);
    }
}
