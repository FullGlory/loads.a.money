using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadBet.Common.Entities;
using SpreadBet.Common.Interfaces;
using Moq;
using SpreadBet.Common.Components;
using SpreadBet.Domain;

namespace SpreadBet.Tests
{
	[TestClass]
	public class ConsistentChangeStockFilterTests
	{
		[TestMethod]
		public void GetInvestmentCandidates_ConsistentPriceIncrease_ReturnsStock()
		{
			var stock = new Stock
				{
                    Identifier = "STK.PLC",
					Name = "STOCK PLC",
					Description = "The Test Stock Price"
				};

			var history = new StockPriceHistory
			{
				Stock = stock,
				Prices = new Dictionary<Period, Price>()
			};

			var currentDate = DateTime.UtcNow.AddMinutes(10);
			var lastPeriodEnd = DateTime.Parse(currentDate.ToString("dd MMMM yyyy HH:00:00"));

            history.Prices.Add(new Period { Id = 1, To = lastPeriodEnd.AddHours(-6) }, new Price { Bid = 25.20m, Offer = 25.26m });
            history.Prices.Add(new Period { Id = 2, To = lastPeriodEnd.AddHours(-5) }, new Price { Bid = 25.20m, Offer = 25.26m });
            history.Prices.Add(new Period { Id = 3, To = lastPeriodEnd.AddHours(-4) }, new Price { Bid = 25.20m, Offer = 25.26m });
            history.Prices.Add(new Period { Id = 4, To = lastPeriodEnd.AddHours(-3) }, new Price { Bid = 25.20m, Offer = 25.26m });
            history.Prices.Add(new Period { Id = 5, To = lastPeriodEnd.AddHours(-2) }, new Price { Bid = 25.20m, Offer = 25.26m });
            history.Prices.Add(new Period { Id = 6, To = lastPeriodEnd.AddHours(-1) }, new Price { Bid = 25.20m, Offer = 25.26m });
            history.Prices.Add(new Period { Id = 7, To = lastPeriodEnd.AddHours(0) }, new Price { Bid = 25.20m, Offer = 25.26m });

			var stockPriceProvider = new Mock<IStockHistoryDataProvider>();
			stockPriceProvider.Setup(x => x.GetStockHistory(stock, 5)).Returns(history);

			var filter = new ConsistentChangeStockFilter(stockPriceProvider.Object, 5);

			var results = filter.GetInvestmentCandidates(new Stock[] { stock });

			Assert.AreEqual(1, results.Count());

		}

		[TestMethod]
		public void GetInvestmentCandidates_LatestPeriodMissing_ReturnsNothing()
		{
			var stock = new Stock
			{
                Identifier = "STK.PLC",
				Name = "STOCK PLC",
				Description = "The Test Stock Price"
			};

			var history = new StockPriceHistory
			{
				Stock = stock,
				Prices = new Dictionary<Period, Price>()
			};

			var currentDate = DateTime.UtcNow.AddMinutes(10);
			var lastPeriodEnd = DateTime.Parse(currentDate.ToString("dd MMMM yyyy HH:00:00"));

            history.Prices.Add(new Period { Id = 1, To = lastPeriodEnd.AddHours(-8) }, new Price { Bid = 45.20m, Offer = 45.26m });
            history.Prices.Add(new Period { Id = 2, To = lastPeriodEnd.AddHours(-7) }, new Price { Bid = 45.20m, Offer = 45.26m });
            history.Prices.Add(new Period { Id = 3, To = lastPeriodEnd.AddHours(-6) }, new Price { Bid = 45.20m, Offer = 45.26m });
            history.Prices.Add(new Period { Id = 4, To = lastPeriodEnd.AddHours(-5) }, new Price { Bid = 45.20m, Offer = 45.26m });
            history.Prices.Add(new Period { Id = 5, To = lastPeriodEnd.AddHours(-4) }, new Price { Bid = 45.20m, Offer = 45.26m });
            history.Prices.Add(new Period { Id = 6, To = lastPeriodEnd.AddHours(-3) }, new Price { Bid = 45.20m, Offer = 45.26m });
            history.Prices.Add(new Period { Id = 7, To = lastPeriodEnd.AddHours(-2) }, new Price { Bid = 45.20m, Offer = 45.26m });

			var stockPriceProvider = new Mock<IStockHistoryDataProvider>();
			stockPriceProvider.Setup(x => x.GetStockHistory(stock, 5)).Returns(history);

			var filter = new ConsistentChangeStockFilter(stockPriceProvider.Object, 5);

			var results = filter.GetInvestmentCandidates(new Stock[] { stock });

			Assert.AreEqual(0, results.Count());

		}

		[TestMethod]
		public void GetInvestmentCandidates_ConsistentPriceDecrease_ReturnsStock()
		{
			var stock = new Stock
			{
                Identifier = "STK.PLC",
				Name = "STOCK PLC",
				Description = "The Test Stock Price"
			};

			var history = new StockPriceHistory
			{
				Stock = stock,
				Prices = new Dictionary<Period, Price>()
			};

			var currentDate = DateTime.UtcNow.AddMinutes(10);
			var lastPeriodEnd = DateTime.Parse(currentDate.ToString("dd MMMM yyyy HH:00:00"));

            history.Prices.Add(new Period { Id = 1, To = lastPeriodEnd.AddHours(-6) }, new Price { Bid = 45.20m, Offer = 45.26m });
            history.Prices.Add(new Period { Id = 2, To = lastPeriodEnd.AddHours(-5) }, new Price { Bid = 43.20m, Offer = 43.26m });
            history.Prices.Add(new Period { Id = 3, To = lastPeriodEnd.AddHours(-4) }, new Price { Bid = 30.20m, Offer = 30.26m });
            history.Prices.Add(new Period { Id = 4, To = lastPeriodEnd.AddHours(-3) }, new Price { Bid = 29.20m, Offer = 29.26m });
            history.Prices.Add(new Period { Id = 5, To = lastPeriodEnd.AddHours(-2) }, new Price { Bid = 27.20m, Offer = 27.30m });
            history.Prices.Add(new Period { Id = 6, To = lastPeriodEnd.AddHours(-1) }, new Price { Bid = 18.00m, Offer = 19.00m });
            history.Prices.Add(new Period { Id = 7, To = lastPeriodEnd.AddHours(0) }, new Price { Bid = 13.00m, Offer = 13.80m });

			var stockPriceProvider = new Mock<IStockHistoryDataProvider>();
			stockPriceProvider.Setup(x => x.GetStockHistory(stock, 5)).Returns(history);

			var filter = new ConsistentChangeStockFilter(stockPriceProvider.Object, 5);

			var results = filter.GetInvestmentCandidates(new Stock[] { stock });

			Assert.AreEqual(1, results.Count());

		}

		[TestMethod]
		public void GetInvestmentCandidates_NotEnoughtPeriods_ReturnsNothing()
		{
			var stock = new Stock
			{
                Identifier = "STK.PLC",
				Name = "STOCK PLC",
				Description = "The Test Stock Price"
			};

			var history = new StockPriceHistory
			{
				Stock = stock,
				Prices = new Dictionary<Period, Price>()
			};

			var currentDate = DateTime.UtcNow.AddMinutes(10);
			var lastPeriodEnd = DateTime.Parse(currentDate.ToString("dd MMMM yyyy HH:00:00"));

            history.Prices.Add(new Period { Id = 5, To = lastPeriodEnd.AddHours(-2) }, new Price { Bid = 27.00m, Offer = 28.00m });
			history.Prices.Add(new Period { Id = 6, To = lastPeriodEnd.AddHours(-1) }, new Price { Bid = 45.20m, Offer = 45.26m });
            history.Prices.Add(new Period { Id = 7, To = lastPeriodEnd.AddHours(0) }, new Price { Bid = 13.00m, Offer = 13.80m });

			var stockPriceProvider = new Mock<IStockHistoryDataProvider>();
			stockPriceProvider.Setup(x => x.GetStockHistory(stock, 5)).Returns(history);

			var filter = new ConsistentChangeStockFilter(stockPriceProvider.Object, 5);

			var results = filter.GetInvestmentCandidates(new Stock[] { stock });

			Assert.AreEqual(0, results.Count());

		}

		[TestMethod]
		public void GetInvestmentCandidates_PriceDecreasesAndIncreases_ReturnsNothing()
		{
			var stock = new Stock
			{
                Identifier = "STK.PLC",
				Name = "STOCK PLC",
				Description = "The Test Stock Price"
			};

			var history = new StockPriceHistory
			{
				Stock = stock,
				Prices = new Dictionary<Period, Price>()
			};

			var currentDate = DateTime.UtcNow.AddMinutes(10);
			var lastPeriodEnd = DateTime.Parse(currentDate.ToString("dd MMMM yyyy HH:00:00"));

            history.Prices.Add(new Period { Id = 1, To = lastPeriodEnd.AddHours(-6) }, new Price { Bid = 45.20m, Offer = 45.26m });
            history.Prices.Add(new Period { Id = 2, To = lastPeriodEnd.AddHours(-5) }, new Price { Bid = 45.20m, Offer = 45.26m });
            history.Prices.Add(new Period { Id = 3, To = lastPeriodEnd.AddHours(-4) }, new Price { Bid = 30.20m, Offer = 30.26m });
            history.Prices.Add(new Period { Id = 4, To = lastPeriodEnd.AddHours(-3) }, new Price { Bid = 29.20m, Offer = 29.26m });
            history.Prices.Add(new Period { Id = 5, To = lastPeriodEnd.AddHours(-2) }, new Price { Bid = 30.00m, Offer = 31.00m });
            history.Prices.Add(new Period { Id = 6, To = lastPeriodEnd.AddHours(-1) }, new Price { Bid = 31.00m, Offer = 32.00m });
            history.Prices.Add(new Period { Id = 7, To = lastPeriodEnd.AddHours(0) }, new Price { Bid = 32.00m, Offer = 32.80m });

			var stockPriceProvider = new Mock<IStockHistoryDataProvider>();
			stockPriceProvider.Setup(x => x.GetStockHistory(stock, 5)).Returns(history);

			var filter = new ConsistentChangeStockFilter(stockPriceProvider.Object, 5);

			var results = filter.GetInvestmentCandidates(new Stock[] { stock });

			Assert.AreEqual(0, results.Count());

		}
	}
}
