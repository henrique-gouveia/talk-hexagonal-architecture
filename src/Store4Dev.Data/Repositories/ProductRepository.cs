using Microsoft.EntityFrameworkCore;
using Store4Dev.Data;
using Store4Dev.Domain.Entities;
using Store4Dev.Domain.Repositories;
using Store4Dev.Domain.Support;

namespace Store4Dev.Data.Repositories
{
    public sealed class ProductRepository : IProductRepository
    {
        private readonly StoreContext storeContext;

        public ProductRepository(StoreContext storeContext)
        {
            Assertion.NotNull(storeContext, "StoreContext must not be null");
            this.storeContext = storeContext;
        }

        public IUnitOfWork UnitOfWork => storeContext;

        public async Task<IEnumerable<Product>> FindAllAsync()
            => await storeContext.Products.AsNoTracking().ToListAsync();

        public async Task<Product> FindOneAsync(Guid id)
            => await storeContext.Products.FindAsync(id);

        public async Task<IEnumerable<Product>> FindByBrandIdAsync(Guid brandId)
            => await storeContext.Products.Where(p => p.Brand.Id == brandId).ToListAsync();

        public async Task SaveAsync(Product product)
        {
            Assertion.NotNull(product, "Product must not be null");

            var entityExistent = await FindOneAsync(product.Id);

            if (entityExistent == null)
                storeContext.Products.Add(product);
            else
                storeContext.Entry(entityExistent).CurrentValues.SetValues(product);
        }
    }
}
