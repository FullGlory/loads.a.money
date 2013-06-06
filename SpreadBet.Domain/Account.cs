// -----------------------------------------------------------------------
// <copyright file="Account.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SpreadBet.Domain
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public class Account : Entity
	{
        public string Username { get; set; }
        public string Password { get; set; }
        public string Url { get; set; }
        public decimal Deposit { get; set; }
	}
}
