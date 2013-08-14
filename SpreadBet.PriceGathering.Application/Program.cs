using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using CuttingEdge.Conditions;
using Microsoft.Practices.Unity;
using SpreadBet.Infrastructure;
using SpreadBet.Infrastructure.Unity;
using SpreadBet.Common.Interfaces;
using Ciloci.Flee;
using SpreadBet.Domain;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Diagnostics;
using SpreadBet.Common.Helpers;
using System.Threading;
using System.ComponentModel;

namespace SpreadBet.PriceGathering.Application
{
	class Program
	{
		static void Main(string[] args)
		{
			using (var processor = new StockProcessor())
			{
				processor.Start();

				System.Console.WriteLine("Press any key to stop");
				System.Console.ReadLine();

				processor.Stop();
			}
		}
	}

	public class StockProcessor: IProcessor, IDisposable
	{
		private readonly IUnityContainer _container;
		private readonly IReceiver<Stock> _receiver;
		private readonly ITransmitter<Stock> _transmitter;

		public StockProcessor()
		{
			this._container = UnityHelper.GetContainer();

			this._receiver = this._container.Resolve<IReceiver<Stock>>();
			this._transmitter = this._container.Resolve<ITransmitter<Stock>>();
		}

		public void Start()
		{
			this._receiver.Start((stock) =>
			{
				this._transmitter.Send(stock);
			});
		}

		public void Stop()
		{
			this._receiver.Stop();
		}

		public void Dispose()
		{
			this._container.Dispose();
		}
	}
}
