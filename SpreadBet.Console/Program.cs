using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpreadBet.Common.Helpers;
using SpreadBet.Common.Interfaces;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using SpreadBet.Repository;

namespace SpreadBet.Console
{
	class Program
	{
		static void Main(string[] args)
		{
			var	container = args.Any() ? UnityHelper.GetContainer(args.First()) : UnityHelper.GetContainer();
			var application = container.Resolve<IExecutableApplication>();

            ResolveDatabase();
			application.Run();
		}

        private static void ResolveDatabase()
        {
            var initialise = new DatabaseInitializer(new Context());

        }
	}
}
