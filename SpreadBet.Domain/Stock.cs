// -----------------------------------------------------------------------
// <copyright file="Stock.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SpreadBet.Domain
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public class Stock : Entity
	{
        public string Identifier { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Security { get; set; }

        public override string ToString()
        {
            return string.Format("Stock [Id: {0}, Identifier: {1}]", Id, Identifier);
        }
	}
}
