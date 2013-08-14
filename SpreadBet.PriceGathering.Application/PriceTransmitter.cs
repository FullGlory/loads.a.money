// -----------------------------------------------------------------------
// <copyright file="PriceTransmitter.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SpreadBet.PriceGathering.Application
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using SpreadBet.Domain;

	public class PriceTransmitter : ITransmitter<StockPrice>
	{
		public void Send(Payload<StockPrice> payload)
		{
			throw new NotImplementedException();
		}
	}
}
