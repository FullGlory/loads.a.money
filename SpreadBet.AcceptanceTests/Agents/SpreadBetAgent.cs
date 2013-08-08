using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadBet.Infrastructure.Data;

namespace SpreadBet.AcceptanceTests.Agents
{
    public class SpreadBetAgent : IPriceDataAgent
    {
        public int GetStockPriceCount()
        {
            using (var ctx = new Context())
            {
                return ctx.StockPrices.Count();
            }
        }
    }
}
