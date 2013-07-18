using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SpreadBet.Domain;
using SpreadBet.Domain.Interfaces;

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
        public void CanSaveAndRetrieveEntities()
        {
            // Arrange
            var account = new Account
            {
                Deposit = 69
            };

            // Act
            this._repository.SaveOrUpdate(account);

            // Assert
            Assert.AreNotEqual(0, account.Id);
            Assert.IsNotNull(this._repository.Get<Account>(a => a.Deposit == 69));
        }

        [Test]
        public void Get_CalledCurrently_DoesNotBlowUp()
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
