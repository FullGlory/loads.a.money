using NUnit.Framework;
using SpreadBet.Domain;
using SpreadBet.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpreadBet.Infrastructure.Logging;

namespace SpreadBet.Infrastructure.Data.IntegrationTests
{
    [TestFixture]
    public class EFRepositoryTests
    {
        private IRepository _repository;

        [SetUp]
        public void SetUp()
        {
            this._repository = new EFRepository();
        }

        [Test]
        public void ShouldSaveAndRetrieveEntities()
        {
            // Arrange
          var seed = Guid.NewGuid().ToString();
            var account = new Account
            {
                Deposit = 69,
                Username = seed
            };

            // Act
            this._repository.SaveOrUpdate(account);

            // Assert
            Assert.AreNotEqual(0, account.Id);
            Assert.IsNotNull(this._repository.Get<Account>(a => a.Username == seed));
        }

        [Test]
        public void ShouldNotRecreateChildEntities()
        {
            // Arrange
            var count = this._repository.GetAll<Stock>().Count();

            var stock = new Stock
            {
                Identifier = Guid.NewGuid().ToString(),
                Description = "x",
                Name = "x",
                Security = "x"

            };
            this._repository.SaveOrUpdate(stock);

            stock = this._repository.Get<Stock>(x => x.Identifier == stock.Identifier);

            var stockPrice = new StockPrice
            {
                Period = new Period
                {
                    From = DateTime.Today,
                    To = DateTime.Today
                },
                Price = new Price
                {
                    Bid = 0,
                    Offer = 0,
                    UpdatedAt = DateTime.Now
                },
                Stock = stock,
                UpdatedAt = DateTime.Now
            };

            // Act
            this._repository.SaveOrUpdate(stockPrice);

            // Assert
            Assert.AreEqual(count + 1, this._repository.GetAll<Stock>().Count(), "Stock has been duplicated");
        }

        [Test]
        public void ShouldUpdateEntities()
        {
          // Arrange
          var count = this._repository.GetAll<Account>().Count();

          var account = new Account
          {
            Deposit = 69
          };

          // Create it
          this._repository.SaveOrUpdate(account);

          account.Deposit += 1;

          // Act
          this._repository.SaveOrUpdate(account);

          // Assert
          Assert.AreEqual(count+1, this._repository.GetAll<Account>().Count());
        }

        [Test]
        public void ShouldBeAbleToCallRepositoryConcurrently()
        {
            // Arrange
            var tasks = new List<Task>();

            // Act
            for(int n=0; n < 3; n++)
            {
                tasks.Add(Task.Factory.StartNew(() => { this._repository.Get<Stock>(s => s.Identifier == "RTR.L"); }));
            }

            Task.WaitAll(tasks.ToArray());

            // Assert
            Assert.IsTrue(tasks.All(t => t.Status == TaskStatus.RanToCompletion));
        }
    }
}
