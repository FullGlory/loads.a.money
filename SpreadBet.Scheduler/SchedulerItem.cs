using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace SpreadBet.Scheduler
{
	/// <summary>
	/// SchedulerItem class
	/// </summary>
	internal abstract class SchedulerItem
	{
		private readonly Timer _timer;
		private readonly TimeSpan _timeout;

		/// <summary>
		/// Initializes a new instance of the <see cref="SchedulerItem"/> class.
		/// </summary>
		/// <param name="timeout">The timeout.</param>
		/// <param name="logger">The logger.</param>
		protected SchedulerItem(TimeSpan timeout)
		{
			this._timeout = timeout;
			this._timer = new Timer(timeout.TotalMilliseconds);
            this._timer.AutoReset = false;  // users of the scheduler have to manually start the scheduler after the elapsed event
			this._timer.Elapsed += this.OnTimerElapsed;
		}

		/// <summary>
		/// Called when [timer elapsed].
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.Timers.ElapsedEventArgs"/> instance containing the event data.</param>
		internal void OnTimerElapsed(object sender, ElapsedEventArgs e)
		{
			ExecuteTask();

            // As the autoreset flag is set to false, we need to restart the timer when the task is finished
            this._timer.Start();
		}

		/// <summary>
		/// Executes the task.
		/// </summary>
		protected abstract void ExecuteTask();

		/// <summary>
		/// Starts the timer.
		/// </summary>
		internal void Start()
		{
			this._timer.Start();
		}

		/// <summary>
		/// Stops the timer.
		/// </summary>
		internal void Stop()
		{
			this._timer.Stop();
		}
	}
}
