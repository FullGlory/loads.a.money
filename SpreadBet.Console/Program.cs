namespace SpreadBet.Console
{
    using SpreadBet.Processor;

    class Program
	{
		static void Main(string[] args)
		{
            var processor = new TradingProcessor();

            processor.Start();

            System.Console.WriteLine("Press any key to stop");
            System.Console.ReadLine();

            processor.Stop();
		}
	}
}
