using Store4Dev.Application.Commands;
using Store4Dev.Application.ViewModels;

namespace Store4Dev.Application.Services
{
    public interface IProductAppService
    {
        Task<IEnumerable<ProductViewModel>> FindAllAsync();
        Task<ProductViewModel> FindOneAsync(Guid productId);
        Task<IEnumerable<ProductViewModel>> FindByBrandIdAsync(Guid brandId);

        Task<ProductViewModel> CreateProductAsync(CreateProductCommand command);

        Task<ProductViewModel> DecreaseStockAsync(Guid productId, decimal quantity);
        Task<ProductViewModel> IncreaseStockAsync(Guid productId, decimal quantity);
    }
}
