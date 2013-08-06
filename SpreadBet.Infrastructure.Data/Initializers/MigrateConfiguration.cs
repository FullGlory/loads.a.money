namespace SpreadBet.Infrastructure.Data.Initializers
{
    using System.Data.Entity.Migrations;
    using SpreadBet.Domain;
    
    /// <summary>
    /// Used to configure the migration of the database
    /// </summary>
    public class MigrateConfiguration : DbMigrationsConfiguration<Context>
    {
        public MigrateConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed(Context context)
        {
            base.Seed(context);

            // It is deliberate thatthis seed method does nothing.
            // See http://blog.oneunicorn.com/2013/05/28/database-initializer-and-migrations-seed-methods/
            // In short, this seed method gets called anytime the database is updated
        }
    }
}
