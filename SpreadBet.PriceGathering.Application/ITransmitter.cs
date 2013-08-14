﻿// -----------------------------------------------------------------------
// <copyright file="ITransmitter.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SpreadBet.PriceGathering.Application
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	/// <summary>
	/// Transmits payload T
	/// </summary>
	/// <typeparam name="T">The type of the payload exchanged between reciever, processor and transmitter</typeparam>
	public interface ITransmitter<T>
	{
		void Send(Payload<T> payload);
	}

}
