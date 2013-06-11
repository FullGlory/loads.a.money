using System;
using System.ServiceProcess;
using SpreadBet.Common.Helpers;
using SpreadBet.Scheduler;
using SpreadBet.Common.Interfaces;
using System.Configuration;
using Microsoft.Practices.Unity;

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

			var thingsToDo = container.ResolveAll<IExecutableApplication>();

            foreach (var thingToDo in thingsToDo)
            {
                scheduler.AddScheduledAction(thingToDo.Run, GetPollingInterval(thingToDo.GetType().Name));
            }

            ServiceBase[] servicesToRun = new ServiceBase[] 
            { 
                new SpreadBetWindowsService(scheduler) 
            };

            ServiceBase.Run(servicesToRun);
		}

		private static TimeSpan GetPollingInterval(string forWhatAction)
		{
			var pollingInterval = 60;
            int.TryParse(ConfigurationManager.AppSettings[string.Format("PollingInterval.{0}", forWhatAction)], out pollingInterval);
			return TimeSpan.FromSeconds(pollingInterval);
		}
	}
}
