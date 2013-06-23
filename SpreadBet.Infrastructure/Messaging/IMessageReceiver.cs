namespace SpreadBet.Infrastructure.Messaging
{
    using System;

    /// <summary>
    /// Abstracts the behavior of a receiving component that raises an event for every received event.
    /// </summary>
    public interface IMessageReceiver
    {
        /// <summary>
        /// Event raised whenever a message is received. Consumer of  the event is responsible for disposing the message when appropriate.
        /// </summary>
        event EventHandler<MessageReceivedEventArgs> MessageReceived;

        /// <summary>
        /// Start the listener
        /// </summary>
        void Start();

        /// <summary>
        /// Stop the listener
        /// </summary>
        void Stop();
    }
}
