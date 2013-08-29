using System;
using System.Collections.Generic;

namespace SpreadBet.Scheduler
{
    /// <summary>
	/// Scheduler class
	/// </summary>
	public class Scheduler : IScheduler
	{
		private readonly List<SchedulerItem> _schedulerItems;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Scheduler"/> class.
		/// </summary>
		public Scheduler()
		{
			this._schedulerItems = new List<SchedulerItem>();
		}

		/// <summary>
		/// Adds the scheduled action.
		/// </summary>
		/// <param name="action">The action.</param>
		/// <param name="timeout">The timeout.</param>
		public void AddScheduledAction(ScheduledActiondDelegate action, TimeSpan timeout)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}

			if (timeout.TotalMilliseconds == 0)
			{
				throw new ArgumentException("timeout is zero", "timeout");
			}

			SchedulerItem item = new DelegateSchedulerItem(action, timeout);
			this._schedulerItems.Add(item);
		}

		/// <summary>
		/// Starts the scheduler.
		/// </summary>
		public void Start()
		{
			foreach (var item in this._schedulerItems)
			{
				item.Start();
			}
		}

		/// <summary>
		/// Stops this scheduler.
		/// </summary>
		public void Stop()
		{
			foreach (var item in this._schedulerItems)
			{
				item.Stop();
			}
		}
	}
}
