// -----------------------------------------------------------------------
// <copyright file="LondonStockExchangePriceFeed.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SpreadBet.Common.Components
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using SpreadBet.Common.Interfaces;
	using System.Text.RegularExpressions;
	using SpreadBet.Common.Entities;
	using SpreadBet.Common.Helpers;
	using System.Threading.Tasks;
	using CuttingEdge.Conditions;
    using SpreadBet.Domain;
    using SpreadBet.Domain.Interfaces;
    using SpreadBet.Infrastructure.Logging;

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
    /// <remarks>
    /// TODO - TPL - http://msdn.microsoft.com/en-us/library/vstudio/jj155756.aspx
    /// </remarks>
	public class LiveCharts_co_uk_PriceFeed : IStockPriceProvider
	{
		private readonly List<string> _listUrls;
		private readonly IScraper _scraper;
        private readonly IRepository _repository;
		private readonly int _timePeriodLengthSecs;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LiveCharts_co_uk_PriceFeed" /> class.
        /// </summary>
        /// <param name="scraper">The scraper.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="timePeriodLengthSecs">The time period length secs.</param>
        /// <param name="logger">The logger.</param>
		public LiveCharts_co_uk_PriceFeed(IScraper scraper, IRepository repository, int timePeriodLengthSecs, ILogger logger)
		{
			Condition.Requires(scraper).IsNotNull();
            Condition.Requires(repository).IsNotNull();
			Condition.Requires(timePeriodLengthSecs).IsGreaterThan(0);
            Condition.Requires(logger).IsNotNull();

            this._repository = repository;
			this._scraper = scraper;
			this._timePeriodLengthSecs = timePeriodLengthSecs;
            this._logger = logger;
				
			#region Set starting urls

			this._listUrls = new string[]
			{
				//"http://www.livecharts.co.uk/share_map.php?letter=0-9", 
                //"http://www.livecharts.co.uk/share_map.php?letter=a", 
                //"http://www.livecharts.co.uk/share_map.php?letter=b", 
                //"http://www.livecharts.co.uk/share_map.php?letter=c", 
                //"http://www.livecharts.co.uk/share_map.php?letter=d", 
                //"http://www.livecharts.co.uk/share_map.php?letter=e", 
                //"http://www.livecharts.co.uk/share_map.php?letter=f", 
                //"http://www.livecharts.co.uk/share_map.php?letter=g", 
                //"http://www.livecharts.co.uk/share_map.php?letter=h", 
                //"http://www.livecharts.co.uk/share_map.php?letter=i", 
                //"http://www.livecharts.co.uk/share_map.php?letter=j", 
                //"http://www.livecharts.co.uk/share_map.php?letter=k", 
                //"http://www.livecharts.co.uk/share_map.php?letter=l", 
                //"http://www.livecharts.co.uk/share_map.php?letter=m", 
                //"http://www.livecharts.co.uk/share_map.php?letter=n", 
                //"http://www.livecharts.co.uk/share_map.php?letter=o", 
                //"http://www.livecharts.co.uk/share_map.php?letter=p", 
                //"http://www.livecharts.co.uk/share_map.php?letter=q", 
                //"http://www.livecharts.co.uk/share_map.php?letter=r", 
                //"http://www.livecharts.co.uk/share_map.php?letter=s",
                //"http://www.livecharts.co.uk/share_map.php?letter=t", 
                //"http://www.livecharts.co.uk/share_map.php?letter=u", 
                //"http://www.livecharts.co.uk/share_map.php?letter=v", 
                //"http://www.livecharts.co.uk/share_map.php?letter=w", 
                //"http://www.livecharts.co.uk/share_map.php?letter=x",
                //"http://www.livecharts.co.uk/share_map.php?letter=y",
                "http://www.livecharts.co.uk/share_map.php?letter=z" 
			}.ToList<string>();

			#endregion
		}

		/// <summary>
		/// Gets the stock prices.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<StockPrice> GetStockPrices()
		{
			var stockPrices = new List<StockPrice>();

            Parallel.ForEach<string>(GetPageUrls(), (url, state) =>
			{
                _logger.Debug("crawling url " + url);

				var stock = ReadStockPrices(url, this._timePeriodLengthSecs);

				if (stock != null)
				{
					lock (stockPrices)
					{
						stockPrices.Add(stock);
					}
				}

                if (stockPrices.Count >= 30) state.Break();
			});

			return stockPrices;
		}

		private List<string> GetPageUrls()
		{
			var retVal = new List<string>();

			var baseUrl = new Uri("http://www.livecharts.co.uk");

			Parallel.ForEach(this._listUrls, url => 
			{
                _logger.Debug("getting stock urls for " + url);

				var response = this._scraper.Scrape(url);
				if (response.Success)
				{
					var urls = Regex.Matches(response.Content, "(?ismx)" + 
															   "<span[^>]*?class\\s?=\\s?[\\\"\\']lookup-one[\\\"\\'][^>]*?>[^<]*?" + 
															   "<a[^>]*?href\\s?=\\s?[\\\"\\'](?<url>share_prices/share_price[^\\\"\\']*)")
								.OfType<Match>()
								.Select(match => new Uri(baseUrl, match.Groups["url"].Value.Trim()).OriginalString);

					retVal.AddRange(urls);
				}
			});

			return retVal;
		}

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

            var stock = _repository.Get<Stock>(s=>s.Identifier.Equals(symb));
            if (stock == null)
            {
                stock = new Stock { Identifier = symb, Name = name, Security = security };
            }
			
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