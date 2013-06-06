// -----------------------------------------------------------------------
// <copyright file="Period.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SpreadBet.Domain
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public class Period : Entity
	{
		public DateTime From { get; set; }
		public DateTime To { get; set; }
	}
}
