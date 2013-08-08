using System.Data.Entity;
using CommandLine;
using SpreadBet.Infrastructure.Data.Initializers;

namespace SpreadBet.Infrastructure.Data.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();

            if (Parser.Default.ParseArguments(args, options))
            {
                System.Console.WriteLine(string.Format("Performing '{0}' of database....", options.DatabaseBuildType.ToString()));

                IDatabaseInitializer<Context> strategy = GetInitializerStrategy(options.DatabaseBuildType);

                Database.SetInitializer(strategy);

                using (var db = new Context())
                {
                    // Use the True flag to force the database initlizer to run
                    db.Database.Initialize(true);
                }

                System.Console.WriteLine(string.Format("'{0}' of database succeeded", options.DatabaseBuildType.ToString()));
            }
        }

        private static IDatabaseInitializer<Context> GetInitializerStrategy(BuildType buildType)
        {
            IDatabaseInitializer<Context> strategy = null;

            switch (buildType)
            {
                case BuildType.Migrate:
                    strategy = new MigrateDatabase();
                    break;
                case BuildType.Rebuild:
                    strategy = new ForceSingleUserInitializer<Context>(new RebuildDatabase());
                    break;
                default:
                    strategy = new NullDatabase();
                    break;
            }

            return strategy;
        }
    }

    /// <summary>
    /// Implementing the null object pattern, this strategy does nothing
    /// </summary>
    public class NullDatabase : IDatabaseInitializer<Context>
    {
        public void InitializeDatabase(Context context)
        {
            // Do nothing
        }
    }

}