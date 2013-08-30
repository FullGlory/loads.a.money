using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SpreadBet.Common.Helpers;
using SpreadBet.Common.Interfaces;
using SpreadBet.Domain;
using SpreadBet.Infrastructure.Logging;

namespace SpreadBet.MarketData
{
    public class LiveChartsStockPriceBot : IStockPriceBot
    {
        private readonly ILogger _logger;
        private readonly IScraper _scraper;
	    private readonly List<string> _stockUrls;
        private bool _stopped;

        public LiveChartsStockPriceBot(ILogger logger, IScraper scraper)
        {
            this._logger = logger;
            this._scraper = scraper;

            #region Setup starting urls

			this._stockUrls = new List<string>(new string[]
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
				});

			#endregion
        }

        public void Scrape(Action<StockPrice> onPriceRead)
        {
            _stopped = false;

            // Get the list of stocks that need to be scraped
			GetStockPageUrls((urls) =>
			{
				// Now start to relay information about each stock
				GetStockPrices(urls, (stockPrice) =>
				{
                    if (!_stopped)
                    {
                        onPriceRead(stockPrice);
                    }
				});
			});
        }

        private void GetStockPageUrls(Action<List<string>> onGetStocksList)
        {
            if (_stopped) return;

            var baseUrl = new Uri("http://www.livecharts.co.uk");

            Parallel.ForEach(this._stockUrls, new ParallelOptions { MaxDegreeOfParallelism = 1 }, (url, state) =>
            {
                if (_stopped)
                {
                    state.Break();
                }
                else
                {
                    this._logger.Debug("getting stock urls for " + url);

                    var response = this._scraper.Scrape(url);
                    if (response.Success)
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

        private void GetStockPrices(IEnumerable<string> pageUrls, Action<StockPrice> onGetStockPrice)
        {
            if (_stopped) return;

            Parallel.ForEach<string>(pageUrls,new ParallelOptions{MaxDegreeOfParallelism=1}, (url, state) =>
            {
                if (_stopped)
                {
                    state.Break();
                }
                else
                {
                    this._logger.Debug("crawling url " + url);

                    var stock = ReadStockPrices(url);

                    if (stock != null)
                    {
                        onGetStockPrice(stock);
                    }
                }
            });
        }

        private StockPrice ReadStockPrices(string url)
        {
            StockPrice retVal = null;

            var response = this._scraper.Scrape(url);
            if (response.Success)
            {
                retVal = this.ExtractStockData(response.Content);
            }

            return retVal;
        }

        private StockPrice ExtractStockData(string content)
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

            var period = TimePeriodHelper.GetTimePeriod(dateTime);

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


        public void Stop()
        {
            _stopped = true;
        }
    }
}
