using SpreadBet.Common.Interfaces;
using SpreadBet.Domain;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium;
using System.Text;

namespace SpreadBet.Common.Components
{
    public class BullBearingsController : IBetController
    {
        private PhantomJSDriver _driver;
        private IJavaScriptExecutor _jsExecutor;

        public BullBearingsController()
        {
            var configOptions = new PhantomJSOptions();
            configOptions.AddAdditionalCapability("--webdriver", 9999);
            configOptions.AddAdditionalCapability("--load-images", false);

            _driver = new PhantomJSDriver(configOptions);
            _jsExecutor = (IJavaScriptExecutor)_driver;
        }

        public bool Open(Domain.Account account, Domain.Bet bet)
        {
            if (this.Login(account))
            {
                _driver.Navigate().GoToUrl("http://www.bullbearings.co.uk/spread.trade.php?epic=" + bet.Stock.Identifier);

                System.IO.File.WriteAllText(@"c:\temp\Bet.html", _driver.PageSource);

                // BUY\SELL
                if (bet.Direction == Direction.Increase)
                    _jsExecutor.ExecuteScript("$('#st #BuyRadio').attr('checked','checked')");
                else
                    _jsExecutor.ExecuteScript("$('#st #SellRadio').attr('checked','checked')");

                // PER POINT
                _jsExecutor.ExecuteScript("$('#st #amountPounds').val('" + bet.BidAmount + "')");

                // DAYS
                _jsExecutor.ExecuteScript("$('#st input[name=days][value=1]').attr('checked','checked')");

                // STOP AT
                _jsExecutor.ExecuteScript("$('#st #stoploss').val('" + bet.ExitPrice +"')");

                // SUBMIT
                _jsExecutor.ExecuteScript("$('#ConfirmBtn').click()");
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
                _jsExecutor.ExecuteScript("$('#ConfirmBtn').click()");
            }

            return true;
        }

        private bool Login(Account account)
        {
            _driver.Navigate().GoToUrl(account.Url);
            _jsExecutor.ExecuteScript("$('#login #email').val('" + account.Username + "')");
            _jsExecutor.ExecuteScript("$('#login #password').val('" + account.Password + "')");
            _jsExecutor.ExecuteScript("$('#login').submit()");

            return true;
        }

        public string Statistics(Account account)
        {
            if (this.Login(account))
            {
                _driver.Navigate().GoToUrl("http://www.bullbearings.co.uk/spread.portfolio.php");

                System.IO.File.WriteAllText(@"c:\temp\Status.html", _driver.PageSource);

                var builder = new StringBuilder();
                builder.Append("var data = [];$('#WrapperContent .portfolioTable tr').each(function(idx, obj){if (obj.cells.length == 9){");
                builder.Append("var item = [];item.push(obj.cells[0]);item.push(obj.cells[1]);item.push(obj.cells[2]); item.push(obj.cells[3]); item.push(obj.cells[4]); item.push(obj.cells[5]);item.push(obj.cells[6]);data.push(item);}});return data;");

                var trades = _driver.ExecuteScript(builder.ToString());
                var summary = _driver.ExecuteScript("var data = [];$('#WrapperContent .tableSummary tr td').each(function(idx, obj){data.push(obj);});return data;");

            }

            // TODO:  SI - need to think how we are gonna pass back current stock update

            return null;
        }
    }
}
