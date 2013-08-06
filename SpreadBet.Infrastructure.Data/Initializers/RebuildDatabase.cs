using System.Data.Entity;
using SpreadBet.Domain;

namespace SpreadBet.Infrastructure.Data.Initializers
{
    public class RebuildDatabase : DropCreateDatabaseAlways<Context>
    {
        /// <summary>
        /// Seed called after the database has been recreated
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed(Context context)
        {
            context.Accounts.Add(new Account { Username = "jpcgoodby@yahoo.co.uk", Password = "trigger", Deposit = 100000 });
        }
    }
}
