using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpreadBet.MarketData
{
    public interface ISender<T>
    {
        void Send(T entity);
    }
}
