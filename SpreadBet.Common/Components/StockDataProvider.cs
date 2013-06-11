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
                // JC - you don't need these 3 updates as the StockPrice is the "aggregate root" 
                // and so will save all the associated entities
                // TODO - should this be done in a tx as a batch is being updated...maybe the Unit Of Work pattern here??
                //_repository.SaveOrUpdate<Period>(sp.Period);
                //_repository.SaveOrUpdate<Stock>(sp.Stock);
                //_repository.SaveOrUpdate<Price>(sp.Price);
                _repository.SaveOrUpdate<StockPrice>(sp);
            }
		}
	}
}
