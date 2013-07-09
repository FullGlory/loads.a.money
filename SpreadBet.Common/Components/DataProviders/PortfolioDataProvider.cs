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
	public class PortfolioDataProvider : IPortfolioDataProvider
	{
		private readonly IRepository _repository;

		public PortfolioDataProvider(IRepository repository)
		{
			Condition.Requires(repository).IsNotNull();

			this._repository = repository;
		}

		public IEnumerable<Bet> GetExistingBets()
		{
            return GetAllBets();
		}

        public IEnumerable<Bet> GetUnplacedBets()
        {
            return GetAllBets();
        }

        private IEnumerable<Bet> GetAllBets()
        {
            return this._repository.GetAll<Bet>(b => b.Stock);
        }

        public void SaveBet(Bet bet)
        {
            this._repository.SaveOrUpdate<Bet>(bet);
        }
		
		public IEnumerable<Bet> GetCurrentBets()
		{
			// TODO Not sure about this tbh.
			return this._repository.GetAll<Bet>(bet => 
				(bet.PlacedOn ?? DateTime.MinValue) < DateTime.UtcNow && 
				(bet.ExitedOn == null), b => b.Stock);
		}


        public Bet Get(int id)
        {
            return this._repository.Get<Bet>(b=>b.Id == id);
        }
    }
}
