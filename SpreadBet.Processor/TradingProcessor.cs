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
        private IUnityContainer _container;
        private List<IProcessor> _processors;

        public TradingProcessor()
        {
            this._container = UnityHelper.GetContainer();

            this._processors = this._container.ResolveAll<IProcessor>().ToList();
        }

        public void Start()
        {
            this._processors.ForEach(p => p.Start());
        }

        public void Stop()
        {
            this._processors.ForEach(p => p.Stop());
        }

        public void Dispose()
        {
            this._container.Dispose();
        }
    }
}
