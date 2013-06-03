using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using SpreadBet.Common.Helpers;
using SpreadBet.Scheduler;
using SpreadBet.Common.Interfaces;
using System.Configuration;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace SpreadBet.WindowsService
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main()
		{
			var container = UnityHelper.GetContainer();

			var scheduler = container.Resolve<IScheduler>();

			var messagingManager = container.Resolve<IExecutableApplication>();

			scheduler.AddScheduledAction(messagingManager.Run, GetPollingInterval());

			ServiceBase[] servicesToRun = new ServiceBase[] 
			{ 
				new SpreadBetWindowsService(scheduler) 
			};

			ServiceBase.Run(servicesToRun);
		}

		private static TimeSpan GetPollingInterval()
		{
			var pollingInterval = 60;
			int.TryParse(ConfigurationManager.AppSettings["pollingInterval"], out pollingInterval);
			return TimeSpan.FromSeconds(pollingInterval);
		}
	}
}
