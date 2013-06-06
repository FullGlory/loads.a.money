// -----------------------------------------------------------------------
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
    using SpreadBet.Repository;
    using SpreadBet.Domain;

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

		public void SaveStockData(IEnumerable<StockPrice> stockPrice)
		{
            foreach(var sp in stockPrice)
            {
                _repository.SaveOrUpdate<Period>(sp.Period);
                _repository.SaveOrUpdate<Stock>(sp.Stock);
                _repository.SaveOrUpdate<Price>(sp.Price);
                _repository.SaveOrUpdate<StockPrice>(sp);
            }
		}
	}
}
