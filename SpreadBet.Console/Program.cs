namespace SpreadBet.Console
{
    using SpreadBet.Domain;
    using SpreadBet.Processor;

    class Program
	{
		static void Main(string[] args)
		{
            using (var processor = new TradingProcessor())
            {
                processor.Start();

                System.Console.WriteLine("Press any key to stop");
                System.Console.ReadLine();

                processor.Stop();
            }
		}
	}
}
