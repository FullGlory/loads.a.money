namespace SpreadBet.Infrastructure.Messaging.Handlers
{
    using System;
    using System.IO;
    using SpreadBet.Infrastructure.Serialisation;

    public abstract class MessageProcessor : IProcessor, IDisposable
    {
        private readonly IMessageReceiver _receiver;
        private readonly ITextSerialiser _serialiser;
        private readonly object lockObject = new object();
        private bool _started;
        private bool _disposed;

        protected MessageProcessor(IMessageReceiver receiver, ITextSerialiser serialiser) 
        {
            this._receiver = receiver;
            this._serialiser = serialiser; 
        }

        ~MessageProcessor() 
        { 
            Dispose(false); 
        }

        public void Dispose()
        {
            Dispose(true); 
            GC.SuppressFinalize(this);
        }

        public void Start()
        {
            lock (this.lockObject) 
            {
                if (!this._started) 
                {
                    this._receiver.MessageReceived += OnMessageReceived;
                    this._receiver.Start();
                    this._started = true;
                }
            }
        }

        private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            var body = Deserialise(e.Message.Body);

            ProcessMessage(body);
        }

        public void Stop()
        {
            lock (this.lockObject) 
            { 
                if (this._started) 
                { 
                    this._receiver.Stop(); 
                    this._receiver.MessageReceived -= OnMessageReceived; 
                    this._started = false; 
                } 
            }
        }

        protected object Deserialise(string serializedPayload) 
        { 
            using (var reader = new StringReader(serializedPayload)) 
            { 
                return this._serialiser.Deserialize(reader); 
            } 
        }

        protected abstract void ProcessMessage(object payload);

        protected virtual void Dispose(bool disposing)        
        {            
            if (!this._disposed)            
            {                
                if (disposing)                
                {                    
                    this.Stop();                    
                    this._disposed = true;                    
                    using (this._receiver as IDisposable)                    
                    {                        
                        // Dispose receiver if it's disposable.                    
                    }                
                }            
            }        
        }
    }
}
