using SpreadBet.Domain;
using System.Collections.Generic;
using SpreadBet.Common.Entities;

namespace SpreadBet.Common.Interfaces
{
    public interface IBetController
    {
        bool Open(Bet bet);
        bool Close(Bet bet);
    }
}
