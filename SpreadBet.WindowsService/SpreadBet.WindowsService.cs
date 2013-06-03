using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using SpreadBet.Scheduler;

namespace SpreadBet.WindowsService
{
	public partial class SpreadBetWindowsService : ServiceBase
	{
	    private IScheduler _scheduler;

		public SpreadBetWindowsService() {}

		public SpreadBetWindowsService(IScheduler scheduler)
        {
            this._scheduler = scheduler;
        }

        protected override void OnStart(string[] args)
        {
            this._scheduler.Start();
        }

        protected override void OnStop()
        {
            this._scheduler.Stop();
        }
	}
}
