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
    using SpreadBet.Repository;

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public class LiveCharts_co_uk_PriceFeed : IStockPriceProvider
	{
		private readonly List<string> _listUrls;
		private readonly IScraper _scraper;
        private readonly IRepository _repository;

		public LiveCharts_co_uk_PriceFeed(IScraper scraper, IRepository repository)
		{
            // FOR FUCKS SAKE!!!!!
			Condition.Requires(scraper).IsNotNull();
            Condition.Requires(repository).IsNotNull();

            this._repository = repository;
			this._scraper = scraper;
				
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

		public IEnumerable<StockPrice> GetStockPrices()
		{
			var timePeriod = TimePeriodHelper.GetTimePeriod(DateTime.UtcNow);

			var stockPrices = new List<StockPrice>();

            //HACK - The intialisation of DbContext in EF is not thread-safe, so restrict parallelism (for now) !!
			Parallel.ForEach<string>(GetPageUrls(), new ParallelOptions{MaxDegreeOfParallelism=1},(url, state)  =>
			{
				Console.WriteLine("crawling url " + url);

				var stock = ReadStockPrices(url, timePeriod);

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
				Console.WriteLine("getting stock urls for " + url);

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

		private StockPrice ExtractStockData(string content, int timePeriod)
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
            var newPeriod = new Period();
            newPeriod.Id = timePeriod;
            newPeriod.From = DateTime.Now;
            newPeriod.To = DateTime.Now.AddHours(1);

            var stock = _repository.Get<Stock>(s=>s.Identifier.Equals(symb));
            if (stock == null)
            {
                stock = new Stock { Identifier = symb, Name = name, Security = security };
            }
			
            return new StockPrice
			{
				Period = newPeriod,
				UpdatedAt = DateTime.UtcNow,
                Stock = stock,
				Price = new Price
				{
					Mid = decimal.Parse(mid), 
					Bid = decimal.Parse(bid), 
					Offer = decimal.Parse(offer), 
					UpdatedAt = DateTime.Now
				}
			};
		}
	}
}