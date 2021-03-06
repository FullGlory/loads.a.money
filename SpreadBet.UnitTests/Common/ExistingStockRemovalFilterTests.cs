﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SpreadBet.Common.Entities;
using SpreadBet.Common.Interfaces;
using Moq;
using SpreadBet.Common.Components;
using SpreadBet.Domain;

namespace SpreadBet.UnitTests.Common
{
	[TestFixture]
	public class ExistingStockRemovalFilterTests
	{
		[Test]
		public void GetInvestmentCandidates_StockAlreadyInvested_ReturnsNothing()
		{
			var stock = new Stock
			{
				Identifier = "STK.PLC",
				Name = "STOCK PLC",
				Description = "The Test Stock Price"
			};

			var stocks = new Stock[] { stock };

			var bet = new Bet
			{
				Stock = stock
			};

			var portfolioProvider = new Mock<IPortfolioDataProvider>();
			portfolioProvider.Setup(x => x.GetExistingBets()).Returns(new Bet[] { bet });

			var filter = new ExistingStockRemovalFilter(portfolioProvider.Object);

			var retVal = filter.GetInvestmentCandidates(stocks);

			Assert.AreEqual(0, retVal.Count());

			portfolioProvider.Verify(x => x.GetExistingBets(), Times.Once());
		}

		[Test]
		public void GetInvestmentCandidates_StockNotAlreadyInvested_ReturnsStock()
		{
			var stock = new Stock
			{
                Identifier = "STK.PLC",
				Name = "STOCK PLC",
				Description = "The Test Stock Price"
			};

			var stocks = new Stock[] { stock };

			var bet = new Bet
			{
				Stock = new Stock
				{
                    Identifier = "NSH.PLC", 
					Name="NOTHING-TO-SEE-HERE PLC", 
					Description = "The Test Stock Price"
				}
			};

			var portfolioProvider = new Mock<IPortfolioDataProvider>();
			portfolioProvider.Setup(x => x.GetExistingBets()).Returns(new Bet[] { bet });

			var filter = new ExistingStockRemovalFilter(portfolioProvider.Object);

			var retVal = filter.GetInvestmentCandidates(stocks);

			Assert.AreEqual(1, retVal.Count());
			Assert.IsTrue(retVal.Any(x => x.Identifier.Equals(stock.Identifier)));

			portfolioProvider.Verify(x => x.GetExistingBets(), Times.Once());
		}
	}
}
