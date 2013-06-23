namespace SpreadBet.WindowsService
{
    using System;
    using System.Configuration;
    using System.ServiceProcess;
    using SpreadBet.Processor;

    static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main()
		{
            var processor = new TradingProcessor();

            ServiceBase[] servicesToRun = new ServiceBase[] 
            { 
                new SpreadBetWindowsService(processor) 
            };

            ServiceBase.Run(servicesToRun);
		}
	}
}
