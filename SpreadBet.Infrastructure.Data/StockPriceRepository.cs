using System;
using System.Transactions;
using SpreadBet.Domain;
using SpreadBet.Domain.Interfaces;

namespace SpreadBet.Infrastructure.Data
{
    public class StockPriceRepository : IStockPriceRepository
    {
        private readonly IRepository _repository;

        public StockPriceRepository(IRepository repository)
        {
            this._repository = repository;
        }

        public void AddStockPrice(StockPrice entity)
        {
            // Stock - We may already know about the stock
            var stock = this._repository.Get<Stock>(s => s.Identifier.Equals(entity.Stock.Identifier));

            if (stock != null)
            {
                entity.Stock = stock;
            }

            // Period - We may already know about the period
            var period = this._repository.Get<Period>(p => p.PeriodId.Equals(entity.Period.PeriodId));

            if (period != null)
            {
                entity.Period = period;
            }
            
            this._repository.SaveOrUpdate<StockPrice>(entity);
        }
    }
}
