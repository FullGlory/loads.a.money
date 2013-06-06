using SpreadBet.Domain;

namespace SpreadBet.Common.Interfaces
{
    public interface IBetController
    {
        bool Open(Account account, Bet bet);
        bool Close(Account account, Bet bet);
        string Statistics(Account account);
    }
}
