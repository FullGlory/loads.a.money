// -----------------------------------------------------------------------
// <copyright file="IPipelineFilter.cs" company="">
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

	/// <summary>
	/// Used to filter the payload returned by the pipeline reciever
	/// </summary>
	public interface IPipelineFilter
	{
		/// <summary>
		/// Evaluates the specified stock.
		/// </summary>
		/// <param name="stock">The stock.</param>
		/// <returns></returns>
		bool Evaluate(Stock stock);
	}
}
