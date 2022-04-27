using Bogus;
using FluentAssertions;
using Store4Dev.Domain.Entities;
using Store4Dev.Domain.Support;
using Xunit;

namespace Store4Dev.Tests.Domain
{
    public class ProductTest
    {
        private readonly Faker<Product> productFaker;

        public ProductTest()
        {
            productFaker = new Faker<Product>()
                .CustomInstantiator(faker => new Product(
                    brand: new Brand("Test Brand"),
                    name: faker.Commerce.ProductName(),
                    costPrice: faker.Random.Int(11, 20),
                    salePrice: faker.Random.Int(21, 30),
                    currentStock: faker.Random.Int(0, 10),
                    minStock: faker.Random.Int(0, 10)
                ));
        }

        [Theory]
        [InlineData(false, "Test Product", 15, 20, 1, 0, "Brand must not be null")]
        [InlineData(true, null, 15, 20, 1, 0, "Name must not be null or empty")]
        [InlineData(true, "", 15, 20, 1, 0, "Name must not be null or empty")]
        [InlineData(true, "Test Product", -1, 20, 1, 0, "Cost Price must not be negative")]
        [InlineData(true, "Test Product", 15, -1, 1, 0, "Sales Price must not be negative")]
        [InlineData(true, "Test Product", 20, 15, 1, 0, "Sales Price must be greater than Cost Price")]
        public void TestRejectsInvalidValues(
            bool createBrand,
            string name,
            decimal costPrice,
            decimal salePrice,
            decimal currentStock,
            decimal minStock,
            string errorMessage)
        {
            var exception = Assert.Throws<DomainException>(() => new Product(
                brand: createBrand ? new Brand("Test Brand") : null,
                name,
                costPrice,
                salePrice,
                currentStock,
                minStock));

            exception.Message.Should().Be(errorMessage);
        }

        [Theory]
        [InlineData(20, 20, 0)]
        [InlineData(20, 40, 100)]
        [InlineData(16, 22, 37.5)]
        public void TestCalcsProfitCorrectly(decimal costPrice, decimal salePrice, decimal profit)
        {
            var product = new Product(
                brand: new Brand("Test Brand"),
                name: "Test Product",
                costPrice,
                salePrice,
                1);

            product.Profit().Should().Be(profit);
        }

        [Fact]
        public void TestRejectsInvalidProfit()
        {
            Product product = productFaker;
            var exception = Assert.Throws<DomainException>(() => product.ChangeProfit(-100));

            exception.Message.Should().Be("Profit must not be negative");
        }

        [Theory]
        [InlineData(20, 0, 20)]
        [InlineData(20, 100, 40)]
        [InlineData(16, 37.5, 22)]
        public void TestCalcsSalePriceCorrectly(decimal costPrice, decimal profit, decimal salePrice)
        {
            var product = new Product(
                brand: new Brand("Test Brand"),
                name: "Test Product",
                costPrice,
                costPrice,
                1);

            product.ChangeProfit(profit);

            product.SalePrice.Should().Be(salePrice);
        }

        [Fact]
        public void TestIncreaseStockRejectsInvalidValue()
        {
            Product product = productFaker;
            var exception = Assert.Throws<DomainException>(() => product.IcreaseStock(-1));

            exception.Message.Should().Be("Value must not be negative");
        }

        [Fact]
        public void TestIncreaseStockCorrectly()
        {
            Product product = productFaker;
            var increaseValue = new Faker().Random.Int(1, 100);
            var expectedStock = product.CurrentStock + increaseValue;

            product.IcreaseStock(increaseValue);

            product.CurrentStock.Should().Be(expectedStock);
        }

        [Fact]
        public void TestDecreaseStockCorrectly()
        {
            Product product = productFaker;
            var decreaseValue = new Faker().Random.Int(1, (int)product.CurrentStock);
            var expectedStock = product.CurrentStock - decreaseValue;

            product.DecreaseStock(decreaseValue);

            product.CurrentStock.Should().Be(expectedStock);
        }

        [Fact]
        public void TestEnableDisable()
        {
            Product product = productFaker;
            product.Active.Should().BeTrue();

            product.Disable();
            product.Active.Should().BeFalse();

            product.Enable();
            product.Active.Should().BeTrue();
        }
    }
}
