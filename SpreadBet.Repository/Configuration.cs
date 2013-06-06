using System.Data.Entity.Migrations;
using SpreadBet.Domain;

namespace SpreadBet.Repository
{
    public class Configuration : DbMigrationsConfiguration<Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Context context)
        {
            context.Account.Add(new Account { Username = "jpcgoodby", Password = "password", Url = "http://www.bullbearings.co.uk/login.php", Deposit = 100000 }); 
        }

    }
}
