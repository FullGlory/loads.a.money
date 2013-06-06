// -----------------------------------------------------------------------
// <copyright file="Price.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SpreadBet.Domain
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    public class Price : Entity
	{
		public decimal Mid { get; set; }
		public decimal Bid { get; set; }
		public decimal Offer { get; set; }

		public DateTime UpdatedAt { get; set; }
	}
}
