﻿// -----------------------------------------------------------------------
// <copyright file="IExitDecider.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SpreadBet.Common.Interfaces
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using SpreadBet.Domain;

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public interface IExitDecider
	{
		IEnumerable<Bet> GetExitDescisions(IEnumerable<Bet> bets);
	}
}
