// -----------------------------------------------------------------------
// <copyright file="StockHistoryDataProvider.cs" company="">
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
	using CuttingEdge.Conditions;

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public class StockHistoryDataProvider: IStockHistoryDataProvider
	{
		private readonly IRepository _repository;

		public StockHistoryDataProvider(IRepository repository)
		{
			Condition.Requires(repository).IsNotNull();

			this._repository = repository;
		}

		public Entities.StockPriceHistory GetStockHistory(Domain.Stock stock)
		{
			// Not sure about this one
			throw new NotImplementedException();
		}

		public Entities.StockPriceHistory GetStockHistory(Domain.Stock stock, int periods)
		{
			// Not sure about this one
			throw new NotImplementedException();
		}
	}
}
