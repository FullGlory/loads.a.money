using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpreadBet.Common.Helpers;
using SpreadBet.Common.Interfaces;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace SpreadBet.Console
{
	class Program
	{
		static void Main(string[] args)
		{
			var container = UnityHelper.GetContainer();

			var application = container.Resolve<IExecutableApplication>();
			application.Run();
		}
	}
}
