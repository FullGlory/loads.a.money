using System;
using SpreadBet.Domain;
using SpreadBet.Domain.Interfaces;

namespace SpreadBet.Infrastructure.Data
{
    public class StockPriceRepository : IStockPriceRepository
    {
        private readonly IRepository repository;

        public StockPriceRepository(IRepository repository)
        {
            this.repository = repository;
        }

        public void AddStockPrice(StockPrice entity)
        {
            // Stock - We may already know about the stock
            var stock = this.repository.Get<Stock>(s => s.Identifier.Equals(entity.Stock.Identifier));

            if (stock != null)
            {
                entity.Stock = stock;
            }

            this.repository.SaveOrUpdate<StockPrice>(entity);
        }
    }
}
