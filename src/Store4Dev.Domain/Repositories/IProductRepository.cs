using Store4Dev.Domain.Entities;
using Store4Dev.Domain.Support;

namespace Store4Dev.Domain.Repositories
{
    public interface IProductRepository : IEntityRepository<Product>
    {
        Task<IEnumerable<Product>> FindAllAsync();
        Task<Product> FindOneAsync(Guid id);
        Task<IEnumerable<Product>> FindByBrandIdAsync(Guid brandId);

        Task SaveAsync(Product product);
    }
}
