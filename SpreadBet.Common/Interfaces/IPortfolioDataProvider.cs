// -----------------------------------------------------------------------
// <copyright file="IPortfolioDataProvider.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SpreadBet.Common.Interfaces
{
    using System.Collections.Generic;
    using SpreadBet.Domain;

	public interface IPortfolioDataProvider
	{
		IEnumerable<Bet> GetExistingBets();

        void SaveBet(Bet bet);

        IEnumerable<Bet> GetUnplacedBets();

		IEnumerable<Bet> GetCurrentBets();

        Bet Get(int id);
    }
}
