namespace SpreadBet.Infrastructure.Data
{
    using System.Data.Entity.Migrations;
    using SpreadBet.Domain;

    public class Configuration : DbMigrationsConfiguration<Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Context context)
        {
            context.Accounts.Add(new Account { Username = "jpcgoodby@yahoo.co.uk", Password = "trigger", Deposit = 100000 }); 
            //context.Accounts.Add(new Account { Username = "bullbearings@wavecrestsoftware.co.uk", Password = "bullbearings", Deposit = 100000 }); 
        }

    }
}
