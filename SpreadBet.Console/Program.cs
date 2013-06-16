using System.Linq;
using Microsoft.Practices.Unity;
using SpreadBet.Common.Helpers;
using SpreadBet.Common.Interfaces;

namespace SpreadBet.Console
{
	class Program
	{
		static void Main(string[] args)
		{
			var	container = args.Any() ? UnityHelper.GetContainer(args.First()) : UnityHelper.GetContainer();
			
            var application = container.Resolve<IExecutableApplication>();

			application.Run();
		}
	}
}
