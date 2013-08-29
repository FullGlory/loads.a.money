using System;

namespace SpreadBet.Infrastructure.Messaging
{
    /// <summary>
    ///  An implementation of a simple Message Channel that connects a sender to a receiver
    /// </summary>
    /// <remarks>
    /// Messaging applications transmit data through a Message Channel, a virtual pipe that connects a sender to a receiver
    /// </remarks>
    /// <typeparam name="T">Type of the data to send</typeparam>
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
