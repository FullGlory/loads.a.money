namespace SpreadBet.Infrastructure.Messaging
{
    /// <summary>
    /// Abstracts the behavior of sending a message.
    /// </summary>
    public interface IMessageSender
    {
        /// <summary>
        /// Sends the specified message.
        /// </summary>
        /// <param name="message"></param>
        void Send(Message message);
    }
}
