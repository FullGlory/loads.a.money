using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace SpreadBet.Repository
{
    public class DatabaseInitializer
    {
        public DatabaseInitializer(Context ctx)
        {
            try
            {
                if (ctx.Database.Exists())
                    ctx.Database.Delete();

                ctx.Database.Initialize(true);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
            }
        }
    }
}
