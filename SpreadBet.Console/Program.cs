namespace SpreadBet.Console
{
    using Microsoft.Practices.Unity;
    using SpreadBet.Common.Interfaces;
    using SpreadBet.Common.Unity;

	class Program
	{
		static void Main(string[] args)
		{
			var	container = UnityHelper.GetContainer();
			
            var application = container.Resolve<IExecutableApplication>();

			application.Run();
		}
	}
}
