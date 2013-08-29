namespace SpreadBet.Infrastructure.Messaging
{
    public interface IReceiver<T>
    {
        void Start(System.Action<T> onReceive);

        void Stop();
    }
}
