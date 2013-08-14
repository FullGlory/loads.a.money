using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Text.RegularExpressions;
using Moq;
using SpreadBet.Common.Interfaces;
using SpreadBet.Common.Entities;
using System.IO;
using SpreadBet.Domain;
using System.Threading;
using System.Diagnostics;
using System.Collections.Concurrent;

namespace SpreadBet.PriceGathering.Application.UnitTests
{
	[TestFixture]
	public class LiveCharts_co_uk_PriceReceiverTests
	{
		private Mock<IScraper> _scraper;
		private Mock<IStockMarket> _stockMarket;
		
		[SetUp]
		public void Setup()
		{
			this._stockMarket = new Mock<IStockMarket>();

			this._scraper = new Mock<IScraper>();

			this._scraper
				.Setup(x => x.Scrape(It.IsRegex(@"(?i)^.*?share_map.php\?letter=.*?$")))
				.Returns<string>((url) =>
				{
					var stockLetter = Regex.Match(url, @"(?i)^.*?share_map.php\?letter=(.*?)$").Groups[1].Value;

					return new ScrapeInfo
					{
						FinalUrl = url,
						Success = true,
						ResponseCode = System.Net.HttpStatusCode.OK,
						Content = GetStockListPage(stockLetter)
					};
				});

			this._scraper
				.Setup(x => x.Scrape(It.IsRegex(@"(?i)^.*?share_prices/share_price/symbol-.*?$")))
				.Returns<string>((url) =>
				{
					var stockSymbol = Regex.Match(url, @"(?i)^.*?share_prices/share_price/symbol-(.*?)$").Groups[1].Value;

					return new ScrapeInfo
					{
						FinalUrl = url,
						Success = true,
						ResponseCode = System.Net.HttpStatusCode.OK,
						Content = GetStockDetailsPage(stockSymbol)
					};

				});
		}

		[Test]
		public void Start_StopIsCalledAfterManyIterations_RunsPerpetually()
		{
			var @lock = new Object();
			var stockLetters = new List<string>();
			var rerunDetected = false;

			this._scraper
				.Setup(x => x.Scrape(It.IsRegex(@"(?i)^.*?share_map.php\?letter=.*?$")))
				.Returns<string>((url) =>
				{
					lock (@lock)
					{
						var stockLetter = Regex.Match(url, @"(?i)^.*?share_map.php\?letter=(.*?)$").Groups[1].Value;

						rerunDetected = rerunDetected || stockLetters.Contains(stockLetter);
						stockLetters.Add(stockLetter);
					}

					return new ScrapeInfo
					{
						FinalUrl = url,
						Success = true,
						Content = string.Empty
					};
				});

			var receiver = new LiveCharts_co_uk_PriceReceiver(this._scraper.Object, this._stockMarket.Object, 30);

			receiver.Start((stock) => { });

			var stopWatch = new Stopwatch();
			stopWatch.Start();
			while (!rerunDetected && (stopWatch.Elapsed < TimeSpan.FromSeconds(60)))
			{
				Thread.Sleep(100);
			}

			receiver.Stop();

			Assert.IsTrue(rerunDetected);
		}

		[Test]
		public void Start_ScraperUsedToRetrieveAllStockNames_VisitsEachStockIndexPage()
		{
			var @lock = new Object();
			var stockLetters = new List<string>("a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,0-9".Split(','));

			this._scraper
				.Setup(x => x.Scrape(It.IsRegex(@"(?i)^.*?share_map.php\?letter=.*?$")))
				.Returns<string>((url) =>
				{
					lock (@lock)
					{
						if (stockLetters.Count > 0)
						{
							var stockLetter = Regex.Match(url, @"(?i)^.*?share_map.php\?letter=(.*?)$").Groups[1].Value;

							CollectionAssert.Contains(stockLetters, stockLetter);
							stockLetters.Remove(stockLetter);
						}
					}

					return new ScrapeInfo
					{
						FinalUrl = url,
						Success = true,
						Content = string.Empty
					};
				});

			var receiver = new LiveCharts_co_uk_PriceReceiver(this._scraper.Object, this._stockMarket.Object, 30);

			receiver.Start((stock) => { });

			var stopWatch = new Stopwatch();
			stopWatch.Start();
			while (stockLetters.Any() && (stopWatch.Elapsed < TimeSpan.FromSeconds(60)))
			{
				Thread.Sleep(100);
			}

			receiver.Stop();

			Assert.IsFalse(stockLetters.Any());
		}

		[Test]
		public void Start_WithFilter_CallsFilterForEachStockOnAListPage()
		{
			var @lock = new Object();
			var completedStockKey = "";

			var filterCalls = new Dictionary<string, int>();
			
			var filter = new Mock<IPipelineFilter>();
			filter
				.Setup(x => x.Evaluate(It.IsAny<Stock>()))
				.Returns<Stock>((stock) =>
				{
					var key = GetStockKey(stock.Identifier);
					if (!filterCalls.ContainsKey(key)) filterCalls.Add(key, 0);

					filterCalls[key]++;

					return true;
				});

			var receiver = new LiveCharts_co_uk_PriceReceiver(this._scraper.Object, this._stockMarket.Object, filter.Object, 30);

			var stockPricesGathered = new List<StockPrice>();

			// The fact that the receiver has been given a stock price means that all of the stocks
			// on the associated list page must have been catalogued at this point and therefore the
			// stock filter must have been applied to all of them.
			receiver.Start((stockPrice) =>
			{
				lock (@lock)
				{
					if (string.IsNullOrEmpty(completedStockKey))
					{
						receiver.Stop();
						completedStockKey = stockPrice.Value.Stock.Identifier.Substring(0, 1).ToLower();
					}
				}
			});

			// Wait for something to happen
			var stopWatch = new Stopwatch();
			stopWatch.Start();
			while (string.IsNullOrEmpty(completedStockKey) && (stopWatch.Elapsed < TimeSpan.FromSeconds(120)))
			{
				Thread.Sleep(1000);
			}

			receiver.Stop();
			
			Assert.IsNotNullOrEmpty(completedStockKey);

			var listPage = GetStockListPage(completedStockKey);
			var numberOfStocks = Regex.Matches(listPage, "(?ismx)<span\\sclass\\s?=\\s?[\\\"\\']lookup-one[\\\"\\']").Count;

			Assert.AreEqual(numberOfStocks, filterCalls[GetStockKey(completedStockKey)]);
		}

		#region Helper Methods

		private string GetStockDetailsPage(string symbol)
		{
			var retVal = string.Empty;

			var assembly = System.Reflection.Assembly.GetExecutingAssembly();
			var xmlStream = assembly.GetManifestResourceStream(string.Format("SpreadBet.PriceGathering.Application.UnitTests.TestData.LiveChartsTestData.symbol-{0}", symbol));
			if (xmlStream == null)
			{
				xmlStream = assembly.GetManifestResourceStream(string.Format("SpreadBet.PriceGathering.Application.UnitTests.TestData.LiveChartsTestData.symbol-ALL", symbol));
			}

			using (var sr = new StreamReader(xmlStream))
			{
				retVal = sr.ReadToEnd();
				retVal = retVal.Replace("#{STOCK.IDENTIFIER}", symbol);
			}

			return retVal;
		}

		/// <summary>
		/// Gets the content of the test stock.
		/// </summary>
		/// <param name="stockLetter">The stock letter.</param>
		/// <returns></returns>
		private string GetStockListPage(string stockLetter)
		{
			var retVal = string.Empty;

			var key = GetStockKey(stockLetter);
			var assembly = System.Reflection.Assembly.GetExecutingAssembly();
			var xmlStream = assembly.GetManifestResourceStream(string.Format("SpreadBet.PriceGathering.Application.UnitTests.TestData.LiveChartsTestData.share_map.php@letter={0}", key));

			using (var sr = new StreamReader(xmlStream))
			{
				retVal = sr.ReadToEnd();
			}

			return retVal;
		}

		/// <summary>
		/// Gets the stock key.
		/// </summary>
		/// <param name="stockIdentifier">The stock identifier.</param>
		/// <returns></returns>
		private string GetStockKey(string stockIdentifier)
		{
			var stockKey = stockIdentifier.Substring(0, 1).ToLower();
			if (Regex.IsMatch(stockKey, @"^\d$")) stockKey = "0-9";

			return stockKey;
		}

		#endregion
	}
}
