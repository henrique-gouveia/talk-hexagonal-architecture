using System;
using System.Threading.Tasks;
using FluentAssertions;
using Store4Dev.Domain.Entities;
using Store4Dev.Domain.Repositories;
using Store4Dev.Domain.Services.Support;
using Store4Dev.Domain.Support;
using Store4Dev.Tests.Support;
using Xunit;

namespace Store4Dev.Tests.Domain.Services
{
    public class MemStockServiceTest
    {
        private readonly Guid existentProductId;
        private readonly IProductRepository productRepository;

        public MemStockServiceTest()
        {
            Product product = new (
                brand: new Brand("Test Brand"),
                name: "Test Product",
                costPrice: 12,
                salePrice: 22,
                currentStock: 0);
            existentProductId = product.Id;

            productRepository = new MemProductRepository();
            productRepository.SaveAsync(product);
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

            var stockService = new StockService(productRepository);
            var isDecreased = await stockService.DecreaseStockAsync(productId, 1);

            Assert.False(isDecreased);
        }

        [Fact]
        public async Task TestDecreaseStockReturnsFalseWhenProductOutOfStock()
        {
            var stockService = new StockService(productRepository);
            var isDecreased = await stockService.DecreaseStockAsync(existentProductId, 1);

            Assert.False(isDecreased);
        }


        [Theory]
        [InlineData(10, 1)]
        [InlineData(10, 5)]
        [InlineData(10, 10)]
        public async Task TestDecreaseStockCorrectly(decimal currentStock, decimal quantity)
        {
            var product = new Product(
                    brand: new Brand("Test Brand"),
                    name: "Test Product",
                    costPrice: 12,
                    salePrice: 22,
                    currentStock);
            var productId = product.Id;
            await productRepository.SaveAsync(product);

            var stockService = new StockService(productRepository);
            var isDecreased = await stockService.DecreaseStockAsync(productId, quantity);

            var expectedStock = currentStock - quantity;
            
            Assert.True(isDecreased);
            product.CurrentStock.Should().Be(expectedStock);
        }

        [Fact]
        public async Task TestIncreaseStockReturnsFalseWhenProductNotFound()
        {
            var productId = Guid.NewGuid();

            var stockService = new StockService(productRepository);
            var isIncreased = await stockService.IncreaseStockAsync(productId, 1);

            Assert.False(isIncreased);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 5)]
        [InlineData(2, 10)]
        public async Task TestIncreaseStockCorrectly(decimal currentStock, decimal quantity)
        {
            var product = new Product(
                    brand: new Brand("Test Brand"),
                    name: "Test Product",
                    costPrice: 12,
                    salePrice: 22,
                    currentStock);
            var productId = product.Id;
            await productRepository.SaveAsync(product);

            var stockService = new StockService(productRepository);
            var isIncreased = await stockService.IncreaseStockAsync(productId, quantity);
            
            var expectedStock = currentStock + quantity;

            Assert.True(isIncreased);
            product.CurrentStock.Should().Be(expectedStock);
        }
    }
}
