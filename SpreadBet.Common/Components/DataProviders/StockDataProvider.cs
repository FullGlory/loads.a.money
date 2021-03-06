﻿// -----------------------------------------------------------------------
// <copyright file="StockDataProvider.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SpreadBet.Common.Components
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using SpreadBet.Common.Interfaces;
    using SpreadBet.Domain;
    using SpreadBet.Domain.Interfaces;

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public class StockDataProvider: IStockDataProvider
	{
        private readonly IRepository _repository;

        public StockDataProvider(IRepository repository)
        {
            _repository = repository;
        }

		public IEnumerable<Stock> GetStocks()
		{
			return _repository.GetAll<Stock>();
		}

        public void SaveStockData(IEnumerable<StockPrice> stockPrice)
        {
            foreach (var sp in stockPrice)
            {
                // TODO - should this be done in a tx as a batch is being updated...maybe the Unit Of Work pattern here??
                _repository.SaveOrUpdate<StockPrice>(sp);
            }
        }

        public void UpdateStockPrice(StockPrice stockPrice)
        {
            _repository.SaveOrUpdate<StockPrice>(stockPrice);
        }

        public Stock GetStock(string identifier)
        {
            return this._repository.Get<Stock>(s => s.Identifier.Equals(identifier));
        }

        public void AddStockPrice(StockPrice stockPrice)
        {
            // Stock - We may already know about the stock
            var stock = this._repository.Get<Stock>(s => s.Identifier.Equals(stockPrice.Stock.Identifier));

            if (stock != null)
            {
                stockPrice.Stock = stock;
            }

            // TODO - Period - We may already know about the period

            _repository.SaveOrUpdate<StockPrice>(stockPrice);
        }
    }
}
