using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpreadBet.Domain;

namespace SpreadBet.MarketData
{
    public class StraightThroughChannel<T> : ISender<T>, IReceiver<T>
    {
        private Action<T> _onReceive;

        public void Send(T entity)
        {
            if (this._onReceive != null)
            {
                this._onReceive(entity);
            }
        }

        public void Start(Action<T> onReceive)
        {
            this._onReceive = onReceive;
        }

        public void Stop()
        {

        }
    }
}
