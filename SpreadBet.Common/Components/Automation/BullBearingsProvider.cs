namespace SpreadBet.Common.Components.Automation
{
    using System;
    using SpreadBet.Common.Interfaces;
    using SpreadBet.Domain;
    using System.Text;
    using SpreadBet.Common.Exceptions;
    using OpenQA.Selenium;
    using System.Linq;
    using SpreadBet.Common.Helpers;

    public class BullBearingsProvider : IAutomationProvider
    {
        private readonly IAutomationDriver _driver;
        private readonly IAutomationSettings _settings;

        public BullBearingsProvider(IAutomationDriver driver, IAutomationSettings settings)
        {
            _driver = driver;
            _settings = settings;

        }

        public bool Open(Bet bet)
        {
            try
            {

                // Route
                _driver.WebDriver.Route(_settings.BaseUrl, _settings.Page, "spread.trade", "epic=" + bet.Stock.Identifier);

                // 1) Trade Type (BUY\SELL)
                var directionElementId = (bet.Direction == Direction.Increase) ? "BuyRadio" : "SellRadio";
                _driver.WebDriver.FindElementById(directionElementId).Click();

                // 2) Bet Value (Pounds per point)
                _driver.WebDriver.FindElementById("amountPounds").SendKeys(Convert.ToInt32(bet.BidAmount).ToString());

                // 3) When to trade
                // Keep at best (Trade will be executed at the current market price)

                // 4) Order valid for
                // Keep for 1 day
                _driver.WebDriver.ExecuteScript("$('#st input[name=days][value=1]').attr('checked','checked')");

                // 5) Stop Loss (Your bet will automatically close if the price falls below this value.)
                if (bet.ExitPrice > 0)
                    _driver.WebDriver.FindElementById("stoploss").SendKeys(bet.ExitPrice.ToString());

                // Submit
                _driver.WebDriver.FindElementById("ConfirmBtn").Click();

                // Confirm
                return _driver.WebDriver.VerifyRoute(_settings.BaseUrl, _settings.Page, "spread.trade.pending.confirm");
                    
            }
            catch (Exception)
            {
                throw new AutomationException("OPEN", ExceptionMessages.__AUTOMATION_OPEN_POSITION);
            }
        }

        public Price Latest(Stock stock)
        {
            try
            {
                // Route
                _driver.WebDriver.Route(_settings.BaseUrl, _settings.Page, "stock.research", "epic=" + stock.Identifier);

                // Select
                var prices = _driver.WebDriver.FindElements(By.CssSelector("#MainPrices tbody tr:nth-child(2) td"));

                // get latest price
                return new Price { Offer = Convert.ToDecimal(prices[0].Text), Bid = Convert.ToDecimal(prices[1].Text)};

            }
            catch
            {
                throw new AutomationException("VALIDATE", ExceptionMessages.__AUTOMATION_VALIDATE);
            }
        }

        public bool Close(Bet bet)
        {
            try
            {
                // Route
                var direction = bet.Direction == Direction.Increase ? "SELL" : "BUY";
                var param = string.Format("epic={0}&bet={1}&amount={2}", bet.Stock.Identifier, direction, bet.BidAmount);

                _driver.WebDriver.Route(_settings.BaseUrl, _settings.Page, "spread.trade", param);

                // Scrape exit price
                var td = (bet.Direction == Direction.Increase) ? "0" : "1";
                var exitPrice = Convert.ToDecimal(_driver.WebDriver.FindElements(By.CssSelector("#AccountSummary tbody tr:nth-child(" + td + ") td"))[0].Text);

                // Submit
                _driver.WebDriver.FindElementById("ConfirmBtn").Click();

                // Update entity
                bet.ExitPrice = exitPrice;
                bet.ExitedOn = DateTime.Now;

                // Confirm
                return _driver.WebDriver.VerifyRoute(_settings.BaseUrl, _settings.Page, "spread.trade.pending.confirm", param);

            }
            catch
            {
                throw new AutomationException("CLOSE", ExceptionMessages.__AUTOMATION_CLOSE_POSITION);
            }
        }

        public bool Authenticate(Account account)
        {
            try
            {
                // Route - to members and leverage session
                if (!_driver.WebDriver.VerifyRoute(_settings.BaseUrl, _settings.Page, "members"))
                {
                    // Authentication required
                    _driver.WebDriver.Route(_settings.BaseUrl, _settings.Page, "login");

                    _driver.WebDriver.Wait(() => _driver.WebDriver.FindElements(By.TagName("input")).Any());
                    // NOTE - the shithead designer of the site decided to put two forms on the page with the same id containing
                    // the same set of controls with the same ids. The only was to differentiate is that the first set of controls
                    // are "not visible", hence the check for the controls we want being "displayed".
                    var fields = _driver.WebDriver.FindElements(By.TagName("input"));

                    fields.Single(f => f.Displayed && f.GetAttribute("id") == "email").SendKeys(account.Username);
                    fields.Single(f => f.Displayed && f.GetAttribute("id") == "password").SendKeys(account.Password);
                    fields.Single(f => f.Displayed && f.GetAttribute("name") == "submit").Click();
                }

                return _driver.WebDriver.VerifyRoute(_settings.BaseUrl, _settings.Page, "members");

            }
            catch (Exception)
            {
                throw new AutomationException("LOGIN", ExceptionMessages.__AUTOMATION_AUTHENTICATION);
            }
        }
    }
}
