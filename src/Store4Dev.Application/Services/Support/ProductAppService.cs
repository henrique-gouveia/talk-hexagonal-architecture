using AutoMapper;
using Store4Dev.Application.Commands;
using Store4Dev.Application.ViewModels;
using Store4Dev.Domain.Entities;
using Store4Dev.Domain.Repositories;
using Store4Dev.Domain.Services;

namespace Store4Dev.Application.Services.Support
{
    public class ProductAppService : IProductAppService
    {
        private readonly IMapper mapper;
        private readonly IStockService stockService;
        private readonly IProductRepository productRepository;

        public ProductAppService(IMapper mapper, IStockService stockService, IProductRepository productRepository)
        {
            this.mapper = mapper ?? throw new ApplicationException("Mapper should not be null.");
            this.stockService = stockService ?? throw new ApplicationException("StockService should not be null.");
            this.productRepository = productRepository ?? throw new ApplicationException("ProdutctRepository should not be null.");
        }

        public async Task<IEnumerable<ProductViewModel>> FindAllAsync()
        {
            var products = await productRepository.FindAllAsync();
            return mapper.Map<IEnumerable<ProductViewModel>>(products);
        }

        public async Task<ProductViewModel> FindOneAsync(Guid productId)
        {
            var product = await productRepository.FindOneAsync(productId);
            return mapper.Map<ProductViewModel>(product);
        }
        public async Task<IEnumerable<ProductViewModel>> FindByBrandIdAsync(Guid brandId)
        {
            var products = await productRepository.FindByBrandIdAsync(brandId);
            return mapper.Map<IEnumerable<ProductViewModel>>(products);
        }

        public async Task<ProductViewModel> CreateProductAsync(CreateProductCommand command)
        {
            var product = mapper.Map<Product>(command);

            await productRepository.SaveAsync(product);
            await productRepository.UnitOfWork.CompleteAsync();

            return mapper.Map<ProductViewModel>(product);
        }

        public async Task<ProductViewModel> IncreaseStockAsync(Guid productId, decimal quantity)
        {
            var stockIncreased = await stockService.IncreaseStockAsync(productId, quantity);
            if (!stockIncreased)
                throw new ApplicationException("Error when trying to increase stock");

            var product = await productRepository.FindOneAsync(productId);
            return mapper.Map<ProductViewModel>(product);
        }

        public async Task<ProductViewModel> DecreaseStockAsync(Guid productId, decimal quantity)
        {
            var stockDecreased = await stockService.DecreaseStockAsync(productId, quantity);
            if (!stockDecreased)
                throw new ApplicationException("Error when trying to decrease stock");

            var product = await productRepository.FindOneAsync(productId);
            return mapper.Map<ProductViewModel>(product);
        }
    }
}
