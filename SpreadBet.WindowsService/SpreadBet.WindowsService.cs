using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using SpreadBet.Processor;
using SpreadBet.Scheduler;

namespace SpreadBet.WindowsService
{
	public partial class SpreadBetWindowsService : ServiceBase
	{
        private TradingProcessor _processor;

		public SpreadBetWindowsService() {}

		public SpreadBetWindowsService(TradingProcessor processor)
        {
            this._processor = processor;
        }

        protected override void OnStart(string[] args)
        {
            this._processor.Start();
        }

        protected override void OnStop()
        {
            this._processor.Stop();
        }
	}
}
