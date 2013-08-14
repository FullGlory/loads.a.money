// -----------------------------------------------------------------------
// <copyright file="Payload.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SpreadBet.PriceGathering.Application
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public abstract class Payload<T>
	{
		public T Value { get; set; }
	}
}
