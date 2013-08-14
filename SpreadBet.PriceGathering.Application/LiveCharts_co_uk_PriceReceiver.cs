// -----------------------------------------------------------------------
// <copyright file="LiveCharts_co_uk_PriceReceiver.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SpreadBet.PriceGathering.Application
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Threading;
	using System.Threading.Tasks;
	using CuttingEdge.Conditions;
	using SpreadBet.Common.Helpers;
	using SpreadBet.Common.Interfaces;
	using SpreadBet.Domain;

	public class LiveCharts_co_uk_PriceReceiver : IReceiver<StockPrice>
	{
		private readonly IScraper _scraper;
		private readonly IStockMarket _stockMarket;
		private readonly IPipelineFilter _filter;
		private readonly int _timePeriodLengthSecs;

		private readonly List<string> _stockUrls;

		private volatile bool _exit = false;
		private EventWaitHandle _workerEventHandle = new EventWaitHandle(false, EventResetMode.ManualReset);

		/// <summary>
		/// Prevents a default instance of the <see cref="LiveCharts_co_uk_PriceReceiver"/> class from being created.
		/// </summary>
		private LiveCharts_co_uk_PriceReceiver()
		{
			#region Setup starting urls

			this._stockUrls = new List<string>(new string[]
				{
					"http://www.livecharts.co.uk/share_map.php?letter=0-9", 
					"http://www.livecharts.co.uk/share_map.php?letter=a", 
					"http://www.livecharts.co.uk/share_map.php?letter=b", 
					"http://www.livecharts.co.uk/share_map.php?letter=c", 
					"http://www.livecharts.co.uk/share_map.php?letter=d", 
					"http://www.livecharts.co.uk/share_map.php?letter=e", 
					"http://www.livecharts.co.uk/share_map.php?letter=f", 
					"http://www.livecharts.co.uk/share_map.php?letter=g", 
					"http://www.livecharts.co.uk/share_map.php?letter=h", 
					"http://www.livecharts.co.uk/share_map.php?letter=i", 
					"http://www.livecharts.co.uk/share_map.php?letter=j", 
					"http://www.livecharts.co.uk/share_map.php?letter=k", 
					"http://www.livecharts.co.uk/share_map.php?letter=l", 
					"http://www.livecharts.co.uk/share_map.php?letter=m", 
					"http://www.livecharts.co.uk/share_map.php?letter=n", 
					"http://www.livecharts.co.uk/share_map.php?letter=o", 
					"http://www.livecharts.co.uk/share_map.php?letter=p", 
					"http://www.livecharts.co.uk/share_map.php?letter=q", 
					"http://www.livecharts.co.uk/share_map.php?letter=r", 
					"http://www.livecharts.co.uk/share_map.php?letter=s",
					"http://www.livecharts.co.uk/share_map.php?letter=t", 
					"http://www.livecharts.co.uk/share_map.php?letter=u", 
					"http://www.livecharts.co.uk/share_map.php?letter=v", 
					"http://www.livecharts.co.uk/share_map.php?letter=w", 
					"http://www.livecharts.co.uk/share_map.php?letter=x",
					"http://www.livecharts.co.uk/share_map.php?letter=y",
					"http://www.livecharts.co.uk/share_map.php?letter=z" 
				});

			#endregion
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LiveCharts_co_uk_PriceReceiver"/> class.
		/// </summary>
		/// <param name="scraper">The scraper.</param>
		/// <param name="stockMarket">The stock market.</param>
		/// <param name="filter">The filter.</param>
		/// <param name="timePeriodLengthSecs">The time period length secs.</param>
		public LiveCharts_co_uk_PriceReceiver(IScraper scraper, IStockMarket stockMarket, IPipelineFilter filter, int timePeriodLengthSecs)
			: this()
		{
			Condition.Requires(scraper).IsNotNull();
			Condition.Requires(stockMarket).IsNotNull();
			Condition.Requires(timePeriodLengthSecs).IsGreaterThan(0);

			this._scraper = scraper;
			this._stockMarket = stockMarket;
			this._filter = filter;

			this._timePeriodLengthSecs = timePeriodLengthSecs;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LiveCharts_co_uk_PriceReceiver"/> class.
		/// </summary>
		/// <param name="scraper">The scraper.</param>
		/// <param name="stockMarket">The stock market.</param>
		/// <param name="timePeriodLengthSecs">The time period length secs.</param>
		public LiveCharts_co_uk_PriceReceiver(IScraper scraper, IStockMarket stockMarket, int timePeriodLengthSecs)
			: this(scraper, stockMarket, null, timePeriodLengthSecs) { }

		/// <summary>
		/// Starts the specified on recieve.
		/// </summary>
		/// <param name="onRecieve">The on recieve.</param>
		public void Start(Action<Payload<StockPrice>> onRecieve)
		{
			var worker = new BackgroundWorker();
			worker.DoWork += (sender, args) =>
			{
				// Get the list of stocks that need to be scraped
				GetStockPageUrls((urls) =>
				{
					// Now start to relay information about each stock
					GetStockPrices(urls, (stockPrice) =>
					{
						if (!this._exit)
						{
							Task.Factory.StartNew(() =>
							{
								onRecieve(new StockPayload
								{
									Value = stockPrice
								});
							});
						}

					});

				});
			};

			worker.RunWorkerCompleted += (sender, args) =>
			{
				if (this._exit)
				{
					this._workerEventHandle.Set();
				}
				else
				{
					worker.RunWorkerAsync();
				}
			};

			worker.RunWorkerAsync();
		}

		/// <summary>
		/// Stops this instance.
		/// </summary>
		public void Stop()
		{
			this._exit = true;
			this._workerEventHandle.WaitOne();

		}

		/// <summary>
		/// Gets the stock page urls.
		/// </summary>
		/// <returns></returns>
		private void GetStockPageUrls(Action<List<string>> onGetStocksList)
		{
			var baseUrl = new Uri("http://www.livecharts.co.uk");

			if (this._exit) return;

			Parallel.ForEach(this._stockUrls, (url, state) =>
			{

				Debug.WriteLine("getting stock urls for " + url);

				if (this._exit)
				{
					state.Break();
				}
				else
				{
					var response = this._scraper.Scrape(url);
					if (response.Success && !this._exit)
					{
						// Get information about the available stocks
						var stocks = Regex.Matches(response.Content, "(?ismx)" +
																	  "<span[^>]*?class\\s?=\\s?[\\\"\\']lookup-one[\\\"\\'][^>]*?>[^<]*?" +
																	  "<a[^>]*?href\\s?=\\s?[\\\"\\'](?<url>share_prices/share_price[^\\\"\\']*)[^>]*?>" +
																	  "\\s*(?<identifier>[^<]*?)\\s*</\\s?a>[^<]*?</\\s?span>[^<]*?" +
																	  "<span[^>]*?class\\s?=\\s?[\\\"\\']lookup-two[\\\"\\'][^>]*?>[^<]*?" +
																	  "<a[^>]*?>\\s*(?<name>[^<]*?)\\s*</\\s?a>")
									.OfType<Match>()
									.Select(match => new
									{
										Url = new Uri(baseUrl, match.Groups["url"].Value.Trim()).OriginalString,
										Stock = new Stock
										{
											Identifier = match.Groups["identifier"].Value,
											Name = match.Groups["name"].Value
										}
									}).ToList();

						// Filter the list
						if (this._filter != null)
						{
							stocks = stocks
										.Where(stock => _filter.Evaluate(stock.Stock))
										.ToList();
						}

						// Assuming we have found some stocks to process then trigger the price gathering stage
						if (stocks.Any())
						{
							var urls = stocks.Select(x => x.Url).ToList();
							onGetStocksList(urls);
						}
					}
				}
			});
		}

		/// <summary>
		/// Gets the stock prices.
		/// </summary>
		/// <param name="pageUrls">The page urls.</param>
		/// <param name="onGetStockPrice">The on get stock price.</param>
		private void GetStockPrices(IEnumerable<string> pageUrls, Action<StockPrice> onGetStockPrice)
		{
			if (this._exit) return;

			Parallel.ForEach<string>(pageUrls, (url, state) =>
			{
				Debug.WriteLine("crawling url " + url);

				if (this._exit)
				{
					state.Break();
				}
				else
				{
					var stock = ReadStockPrices(url, this._timePeriodLengthSecs);

					if (stock != null)
					{
						onGetStockPrice(stock);
					}
				}
			});
		}

		/// <summary>
		/// Reads the stock prices.
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <param name="timePeriod">The time period.</param>
		/// <returns></returns>
		private StockPrice ReadStockPrices(string url, int timePeriod)
		{
			StockPrice retVal = null;

			var response = this._scraper.Scrape(url);
			if (response.Success)
			{
				retVal = this.ExtractStockData(response.Content, timePeriod);
			}

			return retVal;
		}

		/// <summary>
		/// Extracts the stock data.
		/// </summary>
		/// <param name="content">The content.</param>
		/// <param name="timePeriodLengthSecs">The time period length secs.</param>
		/// <returns></returns>
		private StockPrice ExtractStockData(string content, int timePeriodLengthSecs)
		{
			var nameExp = "(?ismx)" +
						  "<h2[^>]*?class\\s?=\\s?[\\\"\\']title[\\\"\\'][^>]*?>[^<]*?" +
						  "(?:<a[^>]*?>)?" +
						  "\\s*(?<val>[^<]*?)\\s*<";

			var name = Regex.Match(content, nameExp).Groups["val"].Value;

			var symExp = "(?ismx)" +
						 "symbol[^<]*?" +
						 "<span[^>]*?class\\s?=\\s?[\\\"\\']shares_one[^>]*?>\\s*" +
						 "(?:<a[^>]*?>[^<]*?)?" +
						 "(?<val>[^<]*?)\\s*<";

			var symb = Regex.Match(content, symExp).Groups["val"].Value;

			var midExp = "(?ismx)" +
						 "share\\sprice[^<]*?" +
						 "<span[^>]*?class\\s?=\\s?[\\\"\\']shares_one[^>]*?>\\s*" +
						 "(?<val>[\\d\\,\\.]+)";

			var mid = Regex.Match(content, midExp).Groups["val"].Value;

			var valExp = "(?ismx)" +
						 "<th[^>]*?>[^<]*?{0}[^<]*?</\\s?th>[^<]*?" +
						 "<td[^>]*?>\\s*(?:<a[^>]*?>)?(?<value>.*?)\\s*</\\s?(?:a|td)>";

			var bid = Regex.Match(content, string.Format(valExp, "bid")).Groups["value"].Value;
			var offer = Regex.Match(content, string.Format(valExp, "offer")).Groups["value"].Value;
			var security = Regex.Match(content, string.Format(valExp, "security")).Groups["value"].Value;

			var changeDateExp = "(?ismx)" +
								"<th[^>]*?>\\s*last\\schange[^<]*</\\s?th>[^<]*?<td[^>]*?>" +
								"(?<month>\\d+)\\s*.\\s*" +
								"(?<day>\\d+)\\s*.\\s*" +
								"(?<hour>\\d+)\\s*.\\s*" +
								"(?<min>\\d+)";

			var changeDateMatch = Regex.Match(content, changeDateExp);
			var dateTime = new DateTime(DateTime.Now.Year,
										int.Parse(changeDateMatch.Groups["month"].Value),
										int.Parse(changeDateMatch.Groups["day"].Value),
										int.Parse(changeDateMatch.Groups["hour"].Value),
										int.Parse(changeDateMatch.Groups["min"].Value),
										0);

			var period = TimePeriodHelper.GetTimePeriod(dateTime, timePeriodLengthSecs);

			var stock = new Stock { Identifier = symb, Name = name, Security = security };

			return new StockPrice
			{
				Period = period,
				UpdatedAt = DateTime.UtcNow,
				Stock = stock,
				Price = new Price
				{
					Bid = decimal.Parse(bid),
					Offer = decimal.Parse(offer),
					UpdatedAt = DateTime.Now
				}
			};
		}
	}
}
