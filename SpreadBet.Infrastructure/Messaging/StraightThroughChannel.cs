namespace SpreadBet.Infrastructure.Messaging
{
    using System;

    /// <summary>
    /// Implementation of a basic messaging channel, when it receives a message, it notifies instantly any
    /// receivers of the receipt of the message. 
    /// </summary>
    public class StraightThroughChannel : IMessageSender, IMessageReceiver
    {
        public void Send(Message message)
        {
            this.MessageReceived(this, new MessageReceivedEventArgs(message));
        }

        public event EventHandler<MessageReceivedEventArgs> MessageReceived = (sender, args) => { };

        public void Start()
        {
            // Do nothing
        }

        public void Stop()
        {
            // Do nothing
        }
    }
}
