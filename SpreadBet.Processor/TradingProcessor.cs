namespace SpreadBet.Processor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Practices.Unity;
    using SpreadBet.Infrastructure;
    using SpreadBet.Infrastructure.Unity;

    public class TradingProcessor : IDisposable
    {
        private IUnityContainer _container;
        private List<IProcessor> _processors;

        private IEnumerable<Task> _tasks;
        private CancellationTokenSource _tokenSource;

        public TradingProcessor()
        {
            // TODO - just dispose of container in this scope?
            this._container = UnityHelper.GetContainer();

            this._processors = this._container.ResolveAll<IProcessor>().ToList();
        }

        public void Start()
        {
            //this._processors.ForEach(p => p.Start());

            if (_tokenSource != null)
            {
                throw new InvalidOperationException("Processor is already started");
            }

            _tokenSource = new CancellationTokenSource();
            var cancellationToken = _tokenSource.Token;

            _tasks = this._processors
                         .Select(x => Task.Factory.StartNew( ()=>
                             {
                                 x.Start();
                             }, cancellationToken));
        }

        public void Stop()
        {
            _tokenSource.Cancel();
            try
            {
                Task.WaitAll(_tasks.ToArray());
            }
            catch (AggregateException /*ae*/)
            {
                // HACK - seems strange to "have" to catch this exception from WaitAll after cancelling the token
            }
            finally
            {
                _tokenSource.Dispose();
                _tokenSource = null;
            }
        }

        public void Dispose()
        {
            this._container.Dispose();
        }
    }
}
