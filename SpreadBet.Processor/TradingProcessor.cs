namespace SpreadBet.Processor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Practices.Unity;
    using SpreadBet.Infrastructure;
    using SpreadBet.Infrastructure.Unity;

    public class TradingProcessor : IDisposable
    {
        private List<IProcessor> _processors;
        private bool _started;

        public TradingProcessor()
        {
            using (var container = UnityHelper.GetContainer())
            {
                this._processors = container.ResolveAll<IProcessor>().ToList();
            }
        }

        public void Start()
        {
            if (!_started)
            {
                foreach (var p in _processors)
                {
                    p.Start();
                }

                _started = true;
            }
        }

        public void Stop()
        {
            if (_started)
            {
                foreach (var p in _processors)
                {
                    p.Stop();
                }

                _started = false;
            }
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
