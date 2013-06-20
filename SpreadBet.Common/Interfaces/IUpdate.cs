namespace SpreadBet.Common.Interfaces
{
    using SpreadBet.Domain;
    using System.Collections.Generic;

    public interface IUpdate
    {
        void Update(IEnumerable<Stock> stocks);
    }
}
