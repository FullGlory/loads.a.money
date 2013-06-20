// -----------------------------------------------------------------------
// <copyright file="CompositeStockFilter.cs" company="">
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
	using CuttingEdge.Conditions;
    using SpreadBet.Domain;

	public class CompositeExitDecider: IExitDecider
	{
		private readonly List<IExitDecider> _deciders;

		public CompositeExitDecider(params IExitDecider[] deciders)
		{
			Condition.Requires(deciders)
				.IsNotNull()
				.IsNotEmpty();

			this._deciders = new List<IExitDecider>(deciders);
		}
		
		public IEnumerable<Bet> GetExitDescisions(IEnumerable<Bet> bets)
		{
			var allBets = new List<Bet>(bets);
			var exitBets = new List<Bet>();

			this._deciders.ForEach(x =>
			{
				if (allBets.Any())
				{
					var tx = x.GetExitDescisions(allBets);

					// Push each exit bet onto the return list
					exitBets.AddRange(tx);

					// Remove each exit bet from the list of existing bets
					tx.ToList().ForEach(y =>
					{
						allBets.Remove(y);
					});
				}
			});

			return exitBets;
		}
	}	
}
