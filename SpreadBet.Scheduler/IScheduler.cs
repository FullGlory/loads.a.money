// -----------------------------------------------------------------------
// <copyright file="IScheduler.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SpreadBet.Scheduler
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	/// <summary>
	/// Schedules the invocation of a delegate at recurring intervals
	/// </summary>
	public interface IScheduler
	{
		/// <summary>
		/// Adds the scheduled action.
		/// </summary>
		/// <param name="action">The action.</param>
		/// <param name="timeout">The timeout.</param>
		void AddScheduledAction(ScheduledActiondDelegate action, TimeSpan timeout);

		/// <summary>
		/// Starts the scheduler.
		/// </summary>
		void Start();

		/// <summary>
		/// Stops this scheduler.
		/// </summary>
		void Stop();
	}

	public delegate void ScheduledActiondDelegate();
}
