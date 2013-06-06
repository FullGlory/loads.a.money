using SpreadBet.Domain;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using System.Data.Entity.Infrastructure;

namespace SpreadBet.Repository
{
	public class Context : DbContext
    {
        public Context() : base("SpreadBet") 
        {

        }

        public DbSet<Account> Account { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Bet> Bets { get; set; }
        public DbSet<Period> Period { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

			ConfigureModel(modelBuilder);

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Context, Configuration>());

			base.OnModelCreating(modelBuilder);
		}

		private void ConfigureModel(DbModelBuilder modelBuilder)
		{
			var entityMethod = typeof(DbModelBuilder).GetMethod("Entity");

			var entityTypes = Assembly.GetAssembly(typeof(Entity)).GetTypes()
				.Where(x => x.IsSubclassOf(typeof(Entity)) && !x.IsAbstract);
			foreach (var type in entityTypes)
			{
				entityMethod.MakeGenericMethod(type).Invoke(modelBuilder, new object[] { });
			}
		}
	}
}
