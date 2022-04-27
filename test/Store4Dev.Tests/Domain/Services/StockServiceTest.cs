using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Store4Dev.Domain.Entities;
using Store4Dev.Domain.Repositories;
using Store4Dev.Domain.Services;
using Store4Dev.Domain.Services.Support;
using Store4Dev.Domain.Support;
using Xunit;

namespace Store4Dev.Tests.Domain.Services
{
    public class StockServiceTest
    {
        private readonly Mock<IUnitOfWork> unitOfWOrkFaker = new();
        private readonly Mock<IProductRepository> productRepositoryFaker = new();

        public StockServiceTest()
        {
            unitOfWOrkFaker
                .Setup(u => u.CompleteAsync())
                .Returns(() => Task.FromResult(true));

            productRepositoryFaker
                .Setup(r => r.UnitOfWork)
                .Returns(unitOfWOrkFaker.Object);
        }

        [Fact]
        public void TestRejectsNullProductRepository()
        {
            var exception = Assert.Throws<DomainException>(() =>
                new StockService(null));

            exception.Message.Should().Be("ProductRepository must not be null");
        }

        [Fact]
        public async Task TestDecreaseStockReturnsFalseWhenProductNotFound()
        {
            var productId = Guid.NewGuid();

            productRepositoryFaker
                .Setup(r => r.FindOneAsync(It.IsAny<Guid>()))
                .Returns(() => Task.FromResult<Product>(null));
            var stockService = new StockService(productRepositoryFaker.Object);

            var isDecreased = await stockService.DecreaseStockAsync(productId, 1);

            Assert.False(isDecreased);
        }

        [Fact]
        public async Task TestDecreaseStockReturnsFalseWhenProductOutOfStock()
        {
            var productId = Guid.NewGuid();

            productRepositoryFaker
                .Setup(r => r.FindOneAsync(productId))
                .Returns(() => Task.FromResult(new Product(
                    brand: new Brand("Test Brand"),
                    name: "Test Product",
                    costPrice: 12,
                    salePrice: 22,
                    currentStock: 0)));
            var stockService = new StockService(productRepositoryFaker.Object);

            var isDecreased = await stockService.DecreaseStockAsync(productId, 1);

            Assert.False(isDecreased);
        }


        [Theory]
        [InlineData(10, 1)]
        [InlineData(10, 5)]
        [InlineData(10, 10)]
        public async Task TestDecreaseStockCorrectly(decimal currentStock, decimal quantity)
        {
            var productId = Guid.NewGuid();
            var expectedStock = currentStock - quantity;
            var product = new Product(
                    brand: new Brand("Test Brand"),
                    name: "Test Product",
                    costPrice: 12,
                    salePrice: 22,
                    currentStock);

            productRepositoryFaker
                .Setup(r => r.FindOneAsync(productId))
                .Returns(() => Task.FromResult(product));

            var stockService = new StockService(productRepositoryFaker.Object);

            var isDecreased = await stockService.DecreaseStockAsync(productId, quantity);

            Assert.True(isDecreased);
            product.CurrentStock.Should().Be(expectedStock);

            productRepositoryFaker.Verify(r => r.FindOneAsync(productId));
            productRepositoryFaker.Verify(r => r.SaveAsync(product));
        }

        [Fact]
        public async Task TestIncreaseStockReturnsFalseWhenProductNotFound()
        {
            var productId = Guid.NewGuid();

            productRepositoryFaker
                .Setup(r => r.FindOneAsync(It.IsAny<Guid>()))
                .Returns(() => Task.FromResult<Product>(null));
            var stockService = new StockService(productRepositoryFaker.Object);

            var isIncreased = await stockService.IncreaseStockAsync(productId, 1);

            Assert.False(isIncreased);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 5)]
        [InlineData(2, 10)]
        public async Task TestIncreaseStockCorrectly(decimal currentStock, decimal quantity)
        {
            var productId = Guid.NewGuid();
            var expectedStock = currentStock + quantity;
            var product = new Product(
                    brand: new Brand("Test Brand"),
                    name: "Test Product",
                    costPrice: 12,
                    salePrice: 22,
                    currentStock);

            productRepositoryFaker
                .Setup(r => r.FindOneAsync(productId))
                .Returns(() => Task.FromResult(product));

            var stockService = new StockService(productRepositoryFaker.Object);

            var isIncreased = await stockService.IncreaseStockAsync(productId, quantity);

            Assert.True(isIncreased);
            product.CurrentStock.Should().Be(expectedStock);

            productRepositoryFaker.Verify(r => r.FindOneAsync(productId));
            productRepositoryFaker.Verify(r => r.SaveAsync(product));
        }
    }
}
