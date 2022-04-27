using AutoMapper;
using ExpectedObjects;
using Store4Dev.Application.AutoMapper;
using Store4Dev.Application.Commands;
using Store4Dev.Application.Services;
using Store4Dev.Application.Services.Support;
using Store4Dev.Application.ViewModels;
using Store4Dev.Domain.Entities;
using Store4Dev.Domain.Repositories;
using Store4Dev.Domain.Services;
using Store4Dev.Domain.Services.Support;
using Store4Dev.Tests.Support;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Store4Dev.Tests.Application.Services
{
    public class ProductAppServiceTest
    {
        private readonly Guid existingProductId;
        private readonly Guid existingBrandId;

        private readonly IMapper mapper;
        private readonly IStockService stockService;
        private readonly IProductRepository productRepository;
        private readonly IProductAppService productService;

        public ProductAppServiceTest()
        {
            var mapperConfig = new MapperConfiguration(c => c.AddProfile<StoreProfile>());
            mapper = mapperConfig.CreateMapper();

            Product product = new(
                brand: new Brand("Test Brand"),
                name: "Test Product",
                costPrice: 12,
                salePrice: 22,
                currentStock: 50);

            existingProductId = product.Id;
            existingBrandId = product.Brand.Id;

            productRepository = new MemProductRepository();
            productRepository.SaveAsync(product);

            stockService = new StockService(productRepository);

            productService = new ProductAppService(mapper, stockService, productRepository);
        }

        [Fact]
        public void TestNewInstanceRejectsNullMapper()
            => Assert.Throws<ApplicationException>(() => new ProductAppService(null, stockService, productRepository));

        [Fact]
        public void TestNewInstanceRejectsNullStockService()
            => Assert.Throws<ApplicationException>(() => new ProductAppService(mapper, null, productRepository));

        [Fact]
        public void TestNewInstanceRejectsNullProductRepository()
            => Assert.Throws<ApplicationException>(() => new ProductAppService(mapper, stockService, null));

        [Fact]
        public async Task TestFindAllCorrectly()
        {
            var allProducts = await productRepository.FindAllAsync();
            var expectedProducts = allProducts
                .Select(p => MakeFrom(p))
                .ToList();

            var products = await productService.FindAllAsync();

            expectedProducts.ToExpectedObject().ShouldEqual(products);
        }

        [Fact]
        public async Task TestFindOneCorrectly()
        {
            var oneProduct = await productRepository.FindOneAsync(existingProductId);
            var expectedProduct = MakeFrom(oneProduct);

            var product = await productService.FindOneAsync(existingProductId);

            expectedProduct.ToExpectedObject().ShouldEqual(product);
        }

        [Fact]
        public async Task TestFindByBrandIdCorrectly()
        {
            var allProducts = await productRepository.FindByBrandIdAsync(existingBrandId);
            var expectedProducts = allProducts
                .Select(p => MakeFrom(p))
                .ToList();

            var products = await productService.FindByBrandIdAsync(existingBrandId);

            expectedProducts.ToExpectedObject().ShouldEqual(products);
        }

        [Fact]
        public async Task TestCreateProductCorrectly()
        {
            CreateProductCommand command = new()
            {
                BrandId = Guid.NewGuid(),
                BrandName = "Test Brand",
                Name = "Test Product",
                CostPrice = 10,
                SalePrice = 15,
                CurrentStock = 100
            };

            var product = await productService.CreateProductAsync(command);

            Assert.False(product.Id == Guid.Empty, "Product Id must not be empty");
            command.ToExpectedObject().ShouldMatch(product);
        }


        [Fact]
        public async Task TestIncreaseStockFailWhenInvalidProduct()
            => await Assert.ThrowsAsync<ApplicationException>(() =>
                productService.IncreaseStockAsync(Guid.NewGuid(), 1));

        [Theory]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(15)]
        public async Task TestIncreaseStockCorrectly(decimal quantity)
        {
            var existingProduct = await productService.FindOneAsync(existingProductId);
            var expectedStock = existingProduct.CurrentStock + quantity;

            await productService.IncreaseStockAsync(existingProductId, quantity);
            var product = await productService.FindOneAsync(existingProductId);

            Assert.Equal(expectedStock, product.CurrentStock);
        }

        [Fact]
        public async Task TestDecreaseStockFailWhenInvalidProduct()
            => await Assert.ThrowsAsync<ApplicationException>(() =>
                productService.DecreaseStockAsync(Guid.NewGuid(), 1));

        [Theory]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(15)]
        public async Task TestDecreaseStockCorrectly(decimal quantity)
        {
            var existingProduct = await productService.FindOneAsync(existingProductId);
            var expectedStock = existingProduct.CurrentStock - quantity;

            await productService.DecreaseStockAsync(existingProductId, quantity);
            var product = await productService.FindOneAsync(existingProductId);

            Assert.Equal(expectedStock, product.CurrentStock);
        }

        private ProductViewModel MakeFrom(Product product)
            => new()
            {
                Id = product.Id,
                Name = product.Name,
                BrandId = product.BrandId,
                BrandName = product.Brand.Name,
                CostPrice = product.CostPrice,
                SalePrice = product.SalePrice,
                CurrentStock = product.CurrentStock,
                MinStock = product.MinStock,
            };
    }
}
