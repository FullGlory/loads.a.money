namespace SpreadBet.Application
{
    using SpreadBet.Infrastructure;
    using SpreadBet.Repository;

    public class MigrateDatabase : IProcessor
    {
        public void Start()
        {
            var initialise = new DatabaseInitializer(new Context());
        }

        public void Stop()
        {
            // Do nothing
        }
    }
}
