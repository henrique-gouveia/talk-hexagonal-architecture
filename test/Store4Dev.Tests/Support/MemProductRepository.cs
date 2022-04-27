using Store4Dev.Domain.Entities;
using Store4Dev.Domain.Repositories;
using Store4Dev.Domain.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store4Dev.Tests.Support
{
    internal sealed class MemProductRepository : IProductRepository
    {
        private IList<Product> products = new List<Product>();

        public IUnitOfWork UnitOfWork => new InternalUnitOfWork();

        public Task<Product> FindOneAsync(Guid id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            return Task.FromResult(product);
        }

        public Task<IEnumerable<Product>> FindAllAsync()
            => Task.FromResult(products.AsEnumerable());

        public Task<IEnumerable<Product>> FindByBrandIdAsync(Guid brandId)
        {
            var productsByBrand = products.Where(p => p.Brand.Id == brandId).ToList();
            return Task.FromResult(productsByBrand.AsEnumerable());
        }

        public async Task SaveAsync(Product product)
        {
            var existent = await FindOneAsync(product.Id);

            if (existent == null)
            {
                products.Add(product);
            }
            else
            {
                products.Remove(existent);
                products.Add(product);
            }
        }

        private class InternalUnitOfWork : IUnitOfWork
        {
            public Task<bool> CompleteAsync() => Task.FromResult(true);
        }
    }
}
