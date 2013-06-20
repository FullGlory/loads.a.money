
using OpenQA.Selenium.Remote;
namespace SpreadBet.Common.Interfaces
{
    public interface IAutomationDriver
    {
        RemoteWebDriver WebDriver { get; }
    }
}
