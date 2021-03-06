﻿using System;
using System.Linq.Expressions;
using Moq;
using NUnit.Framework;
using SpreadBet.Domain;
using SpreadBet.Domain.Interfaces;

namespace SpreadBet.Infrastructure.Data.UnitTests
{
    [TestFixture]
    public class StockPriceRepositoryTests
    {
        [Test]
        public void StockNotFoundWhenAddingNewStockPrice()
        {
            // Arrange
            var mockRepository = new Mock<IRepository>();
            var domainRepository = new StockPriceRepository(mockRepository.Object);
            mockRepository.Setup(x => x.Get<Stock>(It.IsAny<Expression<Func<Stock, bool>>>())).Returns((Stock)null);
            var sp = new StockPrice
            {
                Stock = new Stock { },
                Period = new Period { }
            };

            // Act
            domainRepository.AddStockPrice(sp);

            // Assert
            mockRepository.Verify(x => x.SaveOrUpdate<StockPrice>(sp), Times.Once());
            mockRepository.VerifyAll();
        }

        [Test]
        public void StockFoundWhenAddingNewStockPrice()
        {
            // Arrange
            var mockRepository = new Mock<IRepository>();
            var domainRepository = new StockPriceRepository(mockRepository.Object);
            mockRepository.Setup(x => x.Get<Stock>(It.IsAny<Expression<Func<Stock, bool>>>())).Returns(new Stock { Identifier = "x" });
            var entity = new StockPrice
            {
                Stock = new Stock { },
                Period = new Period { }
            };

            // Act
            domainRepository.AddStockPrice(entity);

            // Assert
            mockRepository.Verify(x => x.SaveOrUpdate<StockPrice>(It.Is<StockPrice>(sp => sp.Stock.Identifier == "x")), Times.Once());
            mockRepository.VerifyAll();
        }

        [Test]
        public void PeriodNotFoundWhenAddingNewStockPrice()
        {
            // Arrange
            var mockRepository = new Mock<IRepository>();
            var domainRepository = new StockPriceRepository(mockRepository.Object);
            mockRepository.Setup(x => x.Get<Period>(It.IsAny<Expression<Func<Period, bool>>>())).Returns((Period)null);
            var sp = new StockPrice
            {
                Stock = new Stock { },
                Period = new Period { }
            };

            // Act
            domainRepository.AddStockPrice(sp);

            // Assert
            mockRepository.Verify(x => x.SaveOrUpdate<StockPrice>(sp), Times.Once());
            mockRepository.VerifyAll();
        }

        [Test]
        public void PeriodFoundWhenAddingNewStockPrice()
        {
            // Arrange
            var mockRepository = new Mock<IRepository>();
            var domainRepository = new StockPriceRepository(mockRepository.Object);
            mockRepository.Setup(x => x.Get<Period>(It.IsAny<Expression<Func<Period, bool>>>())).Returns(new Period { Id=69 });
            var entity = new StockPrice
            {
                Stock = new Stock { },
                Period = new Period { }
            };

            // Act
            domainRepository.AddStockPrice(entity);

            // Assert
            mockRepository.Verify(x => x.SaveOrUpdate<StockPrice>(It.Is<StockPrice>(sp => sp.Period.Id == 69)), Times.Once());
            mockRepository.VerifyAll();
        }
    }
}
