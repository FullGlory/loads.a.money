﻿// -----------------------------------------------------------------------
// <copyright file="Bet.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SpreadBet.Domain
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public class Bet : Entity
	{
		public Stock Stock { get; set; }
		public Price Price { get; set; }
		public Account Account { get; set; }

		public decimal InitialLoss { get; set; }
		public decimal BidAmount { get; set; }
		public decimal OpeningPosition { get; set; }
		public decimal ExitPrice { get; set; }

		public DateTime? PlacedOn { get; set; }
		public DateTime? ExitedOn { get; set; }
		
		public Direction Direction { get; set; }
	}

	public enum Direction
	{
		Increase, 
		Decrease
	}
}

