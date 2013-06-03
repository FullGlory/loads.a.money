// -----------------------------------------------------------------------
// <copyright file="Class1.cs" company="">
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
	/// A scheduler item that invokes a delegate at a specified time interval
	/// </summary>
	internal class DelegateSchedulerItem : SchedulerItem
	{
		private ScheduledActiondDelegate _action;
		private TimeSpan timeout;

		/// <summary>
		/// Initializes a new instance of the <see cref="DelegateSchedulerItem"/> class.
		/// </summary>
		/// <param name="action">The action.</param>
		/// <param name="timeout">The timeout.</param>
		internal DelegateSchedulerItem(ScheduledActiondDelegate action, TimeSpan timeout)
			: base(timeout)
		{
			this._action = action;
		}

		/// <summary>
		/// Executes the task.
		/// </summary>
		protected override void ExecuteTask()
		{
			// Invoke the delegate
			_action();
		}
	}
}
