namespace SpreadBet.Infrastructure.Messaging
{
    public interface ISender<T>
    {
        void Send(T entity);
    }
}
