using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpreadBet.MarketData
{
    public interface IReceiver<T>
    {
        void Start(Action<T> onReceive);

        void Stop();
    }
}
