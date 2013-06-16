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
    using SpreadBet.Domain;

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public class AccountDataProvider : IAccountDataProvider
	{
		private readonly IRepository _repository;

		public AccountDataProvider(IRepository repository)
		{
			Condition.Requires(repository).IsNotNull();

			this._repository = repository;
		}

		/// <summary>
		/// Gets the current position.
		/// </summary>
		/// <returns></returns>
		public Domain.Account GetCurrentPosition()
		{
            return this._repository.GetAll<Account>()
                                   .Last();
		}
	}
}
