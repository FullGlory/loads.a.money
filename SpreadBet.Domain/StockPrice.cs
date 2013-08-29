// -----------------------------------------------------------------------
// <copyright file="StockPrice.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SpreadBet.Domain
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public class StockPrice : Entity
	{
		public Period Period { get; set; }
		public Stock Stock { get; set; }
		public Price Price { get; set; }
		public DateTime UpdatedAt { get; set; }

        public override string ToString()
        {
            return string.Format("StockPrice [Id: {0}, Identifier: {1}, Price: {2}]", Id, Stock.Identifier, Price.Mid);
        }
	}
}
