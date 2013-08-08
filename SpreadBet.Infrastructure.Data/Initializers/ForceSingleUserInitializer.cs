using System.Data.Entity;

namespace SpreadBet.Infrastructure.Data.Initializers
{
    public class ForceSingleUserInitializer<TContext> : IDatabaseInitializer<TContext> where TContext : DbContext
    {
        private readonly IDatabaseInitializer<TContext> _initializer;

        public ForceSingleUserInitializer(IDatabaseInitializer<TContext> innerInitializer)
        {
            _initializer = innerInitializer;
        }

        public void InitializeDatabase(TContext context)
        {
            var dbName = context.Database.Connection.Database;
            context.Database.ExecuteSqlCommand(string.Format("ALTER DATABASE {0} SET SINGLE_USER WITH ROLLBACK IMMEDIATE", dbName));
            _initializer.InitializeDatabase(context);
        }
    }
}
