namespace SpreadBet.Infrastructure.Data
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using SpreadBet.Domain;

	public class Context : DbContext
    {
        public Context() : base("SpreadBet") 
        {}

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Bet> Bets { get; set; }
        public DbSet<Period> Periods { get; set; }
        public DbSet<StockPrice> StockPrices { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

			base.OnModelCreating(modelBuilder);
		}
	}
}
