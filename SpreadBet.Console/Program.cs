namespace SpreadBet.Console
{
    using SpreadBet.Domain;
    using SpreadBet.Processor;

    class Program
	{
		static void Main(string[] args)
		{
            //using (var processor = new TradingProcessor())
            //{
            //    processor.Start();

            //    System.Console.WriteLine("Press any key to stop");
            //    System.Console.ReadLine();

            //    processor.Stop();
            //}

            var repository = new SpreadBet.Infrastructure.Data.EFRepository();
            var stockPriceProvider = new SpreadBet.Common.Components.StockDataProvider(repository);

            var channel = new SpreadBet.MarketData.StraightThroughChannel<StockPrice>();

            var publisher = new SpreadBet.MarketData.Endpoint.PriceFeedPublishProcessor(channel);
            var subscriber = new SpreadBet.MarketData.Endpoint.PriceFeedSubscribeProcessor(channel, stockPriceProvider);

            var processor = new SpreadBet.MarketData.Endpoint.PriceFeedPubSubProcessor(publisher, subscriber);

            processor.Start();


		}
	}
}
