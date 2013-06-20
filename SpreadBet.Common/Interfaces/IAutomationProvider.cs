using SpreadBet.Domain;

namespace SpreadBet.Common.Interfaces
{
    public interface IAutomationProvider
    {
        bool Open(Bet bet);
        Price Latest(Stock stock);
        bool Close(Bet bet);
        bool Authenticate(Account account);
    }
}
