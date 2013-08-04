using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SpreadBet.Common.Entities;
using SpreadBet.Common.Interfaces;
using Moq;
using SpreadBet.Common.Components;
using SpreadBet.Domain;

namespace SpreadBet.Tests
{
    [TestFixture]
    public class MinimumRateOfChangeStockFilterTests
    {
        [Test]
        public void GetInvestmentCandidates_RateOfGrowthSufficient_ReturnsStock()
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
            history.Prices.Add(new Period { Id = 2, To = lastPeriodEnd.AddHours(-5) }, new Price { Bid = 26.20m, Offer = 26.26m });
            history.Prices.Add(new Period { Id = 3, To = lastPeriodEnd.AddHours(-4) }, new Price { Bid = 28.20m, Offer = 28.26m });
            history.Prices.Add(new Period { Id = 4, To = lastPeriodEnd.AddHours(-3) }, new Price { Bid = 30.20m, Offer = 30.26m });
            history.Prices.Add(new Period { Id = 5, To = lastPeriodEnd.AddHours(-2) }, new Price { Bid = 30.00m, Offer = 30.50m });
            history.Prices.Add(new Period { Id = 6, To = lastPeriodEnd.AddHours(-1) }, new Price { Bid = 31.00m, Offer = 32.00m });
            history.Prices.Add(new Period { Id = 7, To = lastPeriodEnd.AddHours(0) }, new Price { Bid = 35.00m, Offer = 35.80m });

            var stockPriceProvider = new Mock<IStockHistoryDataProvider>();
            stockPriceProvider.Setup(x => x.GetStockHistory(stock, 5)).Returns(history);

            var filter = new MinimumRateOfChangeStockFilter(stockPriceProvider.Object, 5, 25m, 25m);

            var results = filter.GetInvestmentCandidates(new Stock[] { stock });

            Assert.AreEqual(1, results.Count());

        }

        [Test]
        public void GetInvestmentCandidates_RateOfGrowthInsufficient_ReturnsNothing()
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
            history.Prices.Add(new Period { Id = 2, To = lastPeriodEnd.AddHours(-5) }, new Price { Bid = 26.20m, Offer = 26.26m });
            history.Prices.Add(new Period { Id = 3, To = lastPeriodEnd.AddHours(-4) }, new Price { Bid = 28.20m, Offer = 28.26m });
            history.Prices.Add(new Period { Id = 4, To = lastPeriodEnd.AddHours(-3) }, new Price { Bid = 30.20m, Offer = 30.26m });
            history.Prices.Add(new Period { Id = 5, To = lastPeriodEnd.AddHours(-2) }, new Price { Bid = 30.00m, Offer = 30.50m });
            history.Prices.Add(new Period { Id = 6, To = lastPeriodEnd.AddHours(-1) }, new Price { Bid = 31.00m, Offer = 32.00m });
            history.Prices.Add(new Period { Id = 7, To = lastPeriodEnd.AddHours(0) }, new Price { Bid = 35.00m, Offer = 35.80m });

            var stockPriceProvider = new Mock<IStockHistoryDataProvider>();
            stockPriceProvider.Setup(x => x.GetStockHistory(stock, 5)).Returns(history);

            var filter = new MinimumRateOfChangeStockFilter(stockPriceProvider.Object, 5, 35m, 35m);

            var results = filter.GetInvestmentCandidates(new Stock[] { stock });

            Assert.AreEqual(0, results.Count());
        }

        [Test]
        public void GetInvestmentCandidates_RateOfReductionInsufficient_ReturnsNothing()
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
            history.Prices.Add(new Period { Id = 2, To = lastPeriodEnd.AddHours(-5) }, new Price { Bid = 24.20m, Offer = 24.26m });
            history.Prices.Add(new Period { Id = 3, To = lastPeriodEnd.AddHours(-4) }, new Price { Bid = 23.20m, Offer = 23.26m });
            history.Prices.Add(new Period { Id = 4, To = lastPeriodEnd.AddHours(-3) }, new Price { Bid = 22.20m, Offer = 22.26m });
            history.Prices.Add(new Period { Id = 5, To = lastPeriodEnd.AddHours(-2) }, new Price { Bid = 21.00m, Offer = 21.50m });
            history.Prices.Add(new Period { Id = 6, To = lastPeriodEnd.AddHours(-1) }, new Price { Bid = 20.00m, Offer = 22.00m });
            history.Prices.Add(new Period { Id = 7, To = lastPeriodEnd.AddHours(0) }, new Price { Bid = 20.00m, Offer = 20.80m });

            var stockPriceProvider = new Mock<IStockHistoryDataProvider>();
            stockPriceProvider.Setup(x => x.GetStockHistory(stock, 5)).Returns(history);

            var filter = new MinimumRateOfChangeStockFilter(stockPriceProvider.Object, 5, 35m, 35m);

            var results = filter.GetInvestmentCandidates(new Stock[] { stock });

            Assert.AreEqual(0, results.Count());
        }

        [Test]
        public void GetInvestmentCandidates_RateOfReductionSufficient_ReturnsStock()
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
            history.Prices.Add(new Period { Id = 2, To = lastPeriodEnd.AddHours(-5) }, new Price { Bid = 24.20m, Offer = 24.26m });
            history.Prices.Add(new Period { Id = 3, To = lastPeriodEnd.AddHours(-4) }, new Price { Bid = 23.20m, Offer = 23.26m });
            history.Prices.Add(new Period { Id = 4, To = lastPeriodEnd.AddHours(-3) }, new Price { Bid = 22.20m, Offer = 22.26m });
            history.Prices.Add(new Period { Id = 5, To = lastPeriodEnd.AddHours(-2) }, new Price { Bid = 21.00m, Offer = 21.50m });
            history.Prices.Add(new Period { Id = 6, To = lastPeriodEnd.AddHours(-1) }, new Price { Bid = 20.00m, Offer = 22.00m });
            history.Prices.Add(new Period { Id = 7, To = lastPeriodEnd.AddHours(0) }, new Price { Bid = 20.00m, Offer = 20.80m });

            var stockPriceProvider = new Mock<IStockHistoryDataProvider>();
            stockPriceProvider.Setup(x => x.GetStockHistory(stock, 5)).Returns(history);

            var filter = new MinimumRateOfChangeStockFilter(stockPriceProvider.Object, 5, 10m, 10m);

            var results = filter.GetInvestmentCandidates(new Stock[] { stock });

            Assert.AreEqual(1, results.Count());
        }

        [Test]
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
            history.Prices.Add(new Period { Id = 2, To = lastPeriodEnd.AddHours(-7) }, new Price { Bid = 43.20m, Offer = 43.26m });
            history.Prices.Add(new Period { Id = 3, To = lastPeriodEnd.AddHours(-6) }, new Price { Bid = 30.20m, Offer = 30.26m });
            history.Prices.Add(new Period { Id = 4, To = lastPeriodEnd.AddHours(-5) }, new Price { Bid = 29.20m, Offer = 29.26m });
            history.Prices.Add(new Period { Id = 5, To = lastPeriodEnd.AddHours(-4) }, new Price { Bid = 27.00m, Offer = 27.50m });
            history.Prices.Add(new Period { Id = 6, To = lastPeriodEnd.AddHours(-3) }, new Price { Bid = 18.00m, Offer = 18.00m });
            history.Prices.Add(new Period { Id = 7, To = lastPeriodEnd.AddHours(-2) }, new Price { Bid = 13.00m, Offer = 13.80m });

            var stockPriceProvider = new Mock<IStockHistoryDataProvider>();
            stockPriceProvider.Setup(x => x.GetStockHistory(stock, 5)).Returns(history);

            var filter = new MinimumRateOfChangeStockFilter(stockPriceProvider.Object, 5, 25m, 25m);

            var results = filter.GetInvestmentCandidates(new Stock[] { stock });

            Assert.AreEqual(0, results.Count());

        }

        [Test]
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

            history.Prices.Add(new Period { Id = 5, To = lastPeriodEnd.AddHours(-2) }, new Price { Bid = 27.00m, Offer = 27.50m });
            history.Prices.Add(new Period { Id = 6, To = lastPeriodEnd.AddHours(-1) }, new Price { Bid = 18.00m, Offer = 18.00m });
            history.Prices.Add(new Period { Id = 7, To = lastPeriodEnd.AddHours(0) }, new Price { Bid = 13.00m, Offer = 13.80m });

            var stockPriceProvider = new Mock<IStockHistoryDataProvider>();
            stockPriceProvider.Setup(x => x.GetStockHistory(stock, 5)).Returns(history);

            var filter = new MinimumRateOfChangeStockFilter(stockPriceProvider.Object, 5, 25m, 25m);

            var results = filter.GetInvestmentCandidates(new Stock[] { stock });

            Assert.AreEqual(0, results.Count());

        }
    }
}
