using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;
using SpreadBet.Common.Entities;
using SpreadBet.Common.Interfaces;
using SpreadBet.Domain;
using SpreadBet.Domain.Interfaces;

namespace SpreadBet.Common.Components
{
    public class StockHistoryDataProvider : IStockHistoryDataProvider
    {
        private readonly IRepository _repository;

        public StockHistoryDataProvider(IRepository repository)
        {
            Condition.Requires(repository).IsNotNull();
            this._repository = repository;
        }

        public StockPriceHistory GetStockHistory(Stock stock)
        {
            return new StockPriceHistory
            {
                Stock = stock,
                Prices = GetStockPricesForStock(stock).ToDictionary(sp => sp.Period, sp => sp.Price)
            };
        }

        public StockPriceHistory GetStockHistory(Stock stock, int periods)
        {
            return new StockPriceHistory
            {
                Stock = stock,
                Prices = GetStockPricesForStock(stock).Take(periods)
                                                      .ToDictionary(sp => sp.Period, sp => sp.Price)
            };
        }

        private IEnumerable<StockPrice> GetStockPricesForStock(Stock stock)
        {
            return _repository.GetAll<StockPrice>(sp => sp.Stock.Identifier.Equals(stock.Identifier), sp=>sp.Period, sp=>sp.Price)
                              .OrderBy(sp => sp.UpdatedAt);
        }
    }
}
