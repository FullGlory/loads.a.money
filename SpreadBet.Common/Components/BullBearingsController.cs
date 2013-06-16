namespace SpreadBet.Common.Components
{
    using SpreadBet.Common.Interfaces;
    using SpreadBet.Domain;
    using OpenQA.Selenium.PhantomJS;
    using OpenQA.Selenium;
    using System.Text;
    using System;
    using OpenQA.Selenium.Remote;
    using System.Linq;

    public class BullBearingsController : IBetController, IDisposable
    {
        private PhantomJSDriver _driver;
        //private IJavaScriptExecutor _jsExecutor;

        //public BetController()
        //{
        //    //_jsExecutor = (IJavaScriptExecutor)_driver;
        //}

        private RemoteWebDriver WebDriver
        {
            get
            {
                if (_driver == null)
                {
                    //var configOptions = new PhantomJSOptions();
                    //configOptions.AddAdditionalCapability("--webdriver", 9999);
                    //configOptions.AddAdditionalCapability("--load-images", false);

                    //_driver = new PhantomJSDriver(configOptions);
                    _driver = new PhantomJSDriver();
                }
                return _driver;
            }
        }

        public bool Open(Domain.Account account, Domain.Bet bet)
        {
            if (this.Login(account))
            {
                _driver.Navigate().GoToUrl("http://www.bullbearings.co.uk/spread.trade.php?epic=" + bet.Stock.Identifier);

                //System.IO.File.WriteAllText(@"c:\temp\Bet.html", _driver.PageSource);

                // 1) Trade Type (BUY\SELL)
                var directionElementId = (bet.Direction == Direction.Increase) ? "BuyRadio" : "SellRadio";
                //if (bet.Direction == Direction.Increase)
                //    _jsExecutor.ExecuteScript("$('#st #BuyRadio').attr('checked','checked')");
                //else
                //    _jsExecutor.ExecuteScript("$('#st #SellRadio').attr('checked','checked')");

                WebDriver.FindElementById(directionElementId).Click();

                // 2) Bet Value (Pounds per point)
                //_jsExecutor.ExecuteScript("$('#st #amountPounds').val('" + bet.BidAmount + "')");
                WebDriver.FindElementById("amountPounds").SendKeys(Convert.ToInt32(bet.BidAmount).ToString());

                // 3) When to trade
                // Keep at best (Trade will be executed at the current market price)

                // 4) Order valid for
                // Keep for 1 day
                //TODO _jsExecutor.ExecuteScript("$('#st input[name=days][value=1]').attr('checked','checked')");

                // 5) Stop Loss (Your bet will automatically close if the price falls below this value.)
                //_jsExecutor.ExecuteScript("$('#st #stoploss').val('" + bet.ExitPrice +"')");
                WebDriver.FindElementById("stoploss").SendKeys(bet.ExitPrice.ToString());

                // SUBMIT
                //_jsExecutor.ExecuteScript("$('#ConfirmBtn').click()");
                WebDriver.FindElementById("ConfirmBtn").Click();

                var screenshot = ((ITakesScreenshot)WebDriver).GetScreenshot();
                screenshot.SaveAsFile("ss.png", System.Drawing.Imaging.ImageFormat.Png);
            }

            return true;
        }

        public bool Close(Domain.Account account, Domain.Bet bet)
        {
            if (this.Login(account))
            {
                var url = "http://www.bullbearings.co.uk/spread.trade.php?epic=" + bet.Stock.Identifier;

                if (bet.Direction == Direction.Increase)
                    url += "SELL&amount=1";
                else
                    url += "BUY&amount=1";

                _driver.Navigate().GoToUrl(url);
                //_jsExecutor.ExecuteScript("$('#ConfirmBtn').click()");
                WebDriver.FindElementById("ConfirmBtn").Click();
            }

            return true;
        }

        private bool Login(Account account)
        {
            WebDriver.Navigate().GoToUrl(account.Url);

            //var screenshot = ((ITakesScreenshot)WebDriver).GetScreenshot();
            //screenshot.SaveAsFile("ss.png", System.Drawing.Imaging.ImageFormat.Png);

            //_jsExecutor.ExecuteScript("$('#login #email').val('" + account.Username + "')");
            //_jsExecutor.ExecuteScript("$('#login #password').val('" + account.Password + "')");
            //_jsExecutor.ExecuteScript("$('#login').submit()");

            //TODO - yuk
            WebDriver.Wait(() => WebDriver.FindElements(By.TagName("input")).Any());

            // NOTE - the shithead designer of the site decided to put two forms on the page with the same id containing
            // the same set of controls with the same ids. The only was to differentiate is that the first set of controls
            // are "not visible", hence the check for the controls we want being "displayed".
            var fields = _driver.FindElements(By.TagName("input"));

            fields.Single(f => f.Displayed && f.GetAttribute("id") == "email").SendKeys(account.Username);
            fields.Single(f => f.Displayed && f.GetAttribute("id") == "password").SendKeys(account.Password);
            fields.Single(f => f.Displayed && f.GetAttribute("name") == "submit").Click();

            //System.IO.File.WriteAllText(@"c:\temp\Login.html", _driver.PageSource);
            return true;
        }

        public string Statistics(Account account)
        {
            if (this.Login(account))
            {
                WebDriver.Navigate().GoToUrl("http://www.bullbearings.co.uk/spread.portfolio.php");

                //System.IO.File.WriteAllText(@"c:\temp\Status.html", _driver.PageSource);

                //var builder = new StringBuilder();
                //builder.Append("var data = [];$('#WrapperContent .portfolioTable tr').each(function(idx, obj){if (obj.cells.length == 9){");
                //builder.Append("var item = [];item.push(obj.cells[0]);item.push(obj.cells[1]);item.push(obj.cells[2]); item.push(obj.cells[3]); item.push(obj.cells[4]); item.push(obj.cells[5]);item.push(obj.cells[6]);data.push(item);}});return data;");

                //var trades = _driver.ExecuteScript(builder.ToString());
                //var summary = _driver.ExecuteScript("var data = [];$('#WrapperContent .tableSummary tr td').each(function(idx, obj){data.push(obj);});return data;");

            }

            // TODO:  SI - need to think how we are gonna pass back current stock update

            return null;
        }

        public void Dispose() 
        {
            Dispose(true);
            GC.SuppressFinalize(this); 
        }

        protected virtual void Dispose(bool disposing) 
        {
            if (disposing) 
            {
                //TODO - dispose "may" quit automatically
                WebDriver.Quit();
                WebDriver.Dispose();
            }
        }

        // Use C# destructor syntax for finalization code.
        ~BullBearingsController()
        {
            // Simply call Dispose(false).
            Dispose (false);
        }
    }
}
