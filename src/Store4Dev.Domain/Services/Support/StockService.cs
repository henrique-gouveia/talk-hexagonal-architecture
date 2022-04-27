using Store4Dev.Domain.Repositories;
using Store4Dev.Domain.Support;

namespace Store4Dev.Domain.Services.Support
{
    public class StockService : IStockService
    {
        private readonly IProductRepository productRepository;

        public StockService(IProductRepository productRepository)
        {
            this.productRepository = productRepository
                ?? throw new DomainException("ProductRepository must not be null");
        }

        public async Task<bool> DecreaseStockAsync(Guid productId, decimal quantity)
        {
            if (!await InternalDecreaseStock(productId, quantity))
                return false;

            return await productRepository.UnitOfWork.CompleteAsync();
        }

        public async Task<bool> InternalDecreaseStock(Guid productId, decimal quantity)
        {
            var product = await productRepository.FindOneAsync(productId);

            if (product is null) return false;
            if (product.CurrentStock < quantity) return false;

            product.DecreaseStock(quantity);
            await productRepository.SaveAsync(product);

            return true;
        }

        public async Task<bool> IncreaseStockAsync(Guid productId, decimal quantity)
        {
            if (!await InternalIncreaseStock(productId, quantity))
                return false;

            return await productRepository.UnitOfWork.CompleteAsync();
        }

        public async Task<bool> InternalIncreaseStock(Guid productId, decimal quantity)
        {
            var product = await productRepository.FindOneAsync(productId);

            if (product == null)
                return false;

            product.IcreaseStock(quantity);
            await productRepository.SaveAsync(product);

            return true;
        }
    }
}
