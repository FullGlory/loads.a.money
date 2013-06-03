using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadBet.Common.Entities;
using SpreadBet.Common.Interfaces;
using Moq;
using SpreadBet.Common.Components;

namespace SpreadBet.Tests
{
	[TestClass]
	public class DecisionByGrowthAndCurrentPositionTests
	{
		[TestMethod]
		public void GetInvestmentDecisions_BettingOnIncrease_ReturnsBet()
		{
			var stock = new Stock
				{
					Id = "STK.PLC",
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

			history.Prices.Add(new Period { Id = 1, To = lastPeriodEnd.AddHours(-6) }, new Price { Mid = 25.23m, Bid = 27.23m, Offer = 23.23m });
			history.Prices.Add(new Period { Id = 2, To = lastPeriodEnd.AddHours(-5) }, new Price { Mid = 26.23m, Bid = 28.23m, Offer = 24.23m });
			history.Prices.Add(new Period { Id = 3, To = lastPeriodEnd.AddHours(-4) }, new Price { Mid = 28.23m, Bid = 30.23m, Offer = 26.23m });
			history.Prices.Add(new Period { Id = 4, To = lastPeriodEnd.AddHours(-3) }, new Price { Mid = 30.23m, Bid = 32.23m, Offer = 28.23m });
			history.Prices.Add(new Period { Id = 5, To = lastPeriodEnd.AddHours(-2) }, new Price { Mid = 30.25m, Bid = 32.23m, Offer = 28.23m });
			history.Prices.Add(new Period { Id = 6, To = lastPeriodEnd.AddHours(-1) }, new Price { Mid = 31.50m, Bid = 33.23m, Offer = 29.23m });
			history.Prices.Add(new Period { Id = 7, To = lastPeriodEnd.AddHours(0) }, new Price { Mid = 35.40m, Bid = 37.23m, Offer = 33.23m });

			var stockHistoryProvider = new Mock<IStockHistoryDataProvider>();
			stockHistoryProvider.Setup(x => x.GetStockHistory(stock, 5)).Returns(history);

			var accountDataProvider = new Mock<IAccountDataProvider>();

			// We are willing to bet up to £100 per point, but only willing to loose £200, Also we accept that up to 75% of this loss can be because of the spread
			
			// GIVEN that our position is that the price will increase 
			// AND that the BID price is 37.23 
			// AND the SPREAD is 4 points
			// WHEN we are willing to loose up to £150.00 on the spread
			// THEN the maximum bid we can afford to place is 150 / 4 = £37.23 (£37.00 giving an initial loss of £140.00)
 			
			// GIVEN that our position is that the price will increase
			// AND that our bid amount is £37.00 per point
			// AND the maximum amount that we are willing to loose is £200.00
			// WHEN the BID price is 37.23
			// THEN the exit price will be 37.23 - (200 / 10) = 31.82
			
			var decisionMaker = new DecisionByGrowthAndCurrentPosition(stockHistoryProvider.Object, accountDataProvider.Object, 5, 100m, 200m, .75m);
			var bets = decisionMaker.GetInvestmentDescisions(new Stock[] { stock });

			var bet = bets.FirstOrDefault();

			Assert.IsNotNull(bet);
			Assert.AreEqual(37, bet.BidAmount);
			Assert.AreEqual(31.82m, bet.ExitPrice);
			Assert.AreEqual(37.23m, bet.OpeningPosition);
			Assert.AreEqual(Direction.Increase, bet.Direction);
		}

		[TestMethod]
		public void GetInvestmentDecisions_BettingOnDecrease_ReturnsBet()
		{
			var stock = new Stock
			{
				Id = "STK.PLC",
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

			history.Prices.Add(new Period { Id = 1, To = lastPeriodEnd.AddHours(-6) }, new Price { Mid = 35.40m, Bid = 37.23m, Offer = 33.23m });
			history.Prices.Add(new Period { Id = 2, To = lastPeriodEnd.AddHours(-5) }, new Price { Mid = 31.50m, Bid = 33.23m, Offer = 29.23m });
			history.Prices.Add(new Period { Id = 3, To = lastPeriodEnd.AddHours(-4) }, new Price { Mid = 30.25m, Bid = 32.23m, Offer = 28.23m });
			history.Prices.Add(new Period { Id = 4, To = lastPeriodEnd.AddHours(-3) }, new Price { Mid = 30.23m, Bid = 32.23m, Offer = 28.23m });
			history.Prices.Add(new Period { Id = 5, To = lastPeriodEnd.AddHours(-2) }, new Price { Mid = 28.23m, Bid = 30.23m, Offer = 26.23m });
			history.Prices.Add(new Period { Id = 6, To = lastPeriodEnd.AddHours(-1) }, new Price { Mid = 26.23m, Bid = 28.23m, Offer = 24.23m });
			history.Prices.Add(new Period { Id = 7, To = lastPeriodEnd.AddHours(0) }, new Price { Mid = 25.23m, Bid = 27.23m, Offer = 23.23m });

			var stockHistoryProvider = new Mock<IStockHistoryDataProvider>();
			stockHistoryProvider.Setup(x => x.GetStockHistory(stock, 5)).Returns(history);
			 
			var accountDataProvider = new Mock<IAccountDataProvider>();

			// We are willing to bet up to £100 per point, but only willing to loose £200, Also we accept that up to 75% of this loss can be because of the spread

			// GIVEN that our position is that the price will decrease 
			// AND that the OFFER price is 23.23 
			// AND the SPREAD is 4 points
			// WHEN we are willing to loose up to £150.00 on the spread
			// THEN the maximum bid we can afford to place is 150 / 4 = £37.23 (£37.00 giving an initial loss of £140.00)

			// GIVEN that our position is that the price will decrease
			// AND that our bid amount is £37.00 per point
			// AND the maximum amount that we are willing to loose is £200.00
			// WHEN the OFFER price is 23.23
			// THEN the exit price will be 23.23 + (200 / 10) = 28.64

			var decisionMaker = new DecisionByGrowthAndCurrentPosition(stockHistoryProvider.Object, accountDataProvider.Object, 5, 100m, 200m, .75m);
			var bets = decisionMaker.GetInvestmentDescisions(new Stock[] { stock });

			var bet = bets.FirstOrDefault();

			Assert.IsNotNull(bet);
			Assert.AreEqual(37, bet.BidAmount);
			Assert.AreEqual(28.64m, bet.ExitPrice);
			Assert.AreEqual(23.23m, bet.OpeningPosition);
			Assert.AreEqual(Direction.Decrease, bet.Direction);
		}
	}
}
