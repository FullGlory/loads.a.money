// -----------------------------------------------------------------------
// <copyright file="AccountDataProvider.cs" company="">
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
	public class PortfolioDataProvider : IPortfolioDataProvider
	{
		private readonly IRepository _repository;

		public PortfolioDataProvider(IRepository repository)
		{
			Condition.Requires(repository).IsNotNull();

			this._repository = repository;
		}

		public IEnumerable<Domain.Bet> GetExistingBets()
		{
			// TODO: Need to implement this
			return new List<Domain.Bet>();
		}
	}
}
