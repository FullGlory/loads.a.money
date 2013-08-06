using System.Data.Entity;

namespace SpreadBet.Infrastructure.Data.Initializers
{
    /// <summary>
    /// When used, migrates the database to the latest version, creating the database if it does not exist
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public class MigrateDatabase : MigrateDatabaseToLatestVersion<Context, MigrateConfiguration>
    {}
}
