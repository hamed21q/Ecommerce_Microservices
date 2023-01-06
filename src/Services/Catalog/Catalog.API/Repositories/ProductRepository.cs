using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext context;

        public ProductRepository(ICatalogContext context)
        {
            this.context = context;
        }

        public async Task Add(Product product)
        {
            await context.Products.InsertOneAsync(product); 
        }

        public async Task<bool> Delete(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);
            DeleteResult result = await context
                                        .Products
                                        .DeleteOneAsync(filter);

            return result.IsAcknowledged 
                && result.DeletedCount> 0;

        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await context
                        .Products
                        .Find(p => true)
                        .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(string category)
        {
            FilterDefinition<Product> filter = Builders<Product>
                                                .Filter
                                                .Eq(p => p.Category, category);
            return await context
                        .Products
                        .Find(filter)
                        .ToListAsync();
        }

        public async Task<Product> GetByIdAsync(string id)
        {
            return await context
                        .Products
                        .Find(p => p.Id == id)
                        .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetByNameAsync(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);
            return await context
                        .Products.Find(filter)
                        .ToListAsync();
        }

        public async Task<bool> Update(Product product)
        {
            var updateResult = await context
                                .Products
                                .ReplaceOneAsync(filter: p => p.Id == product.Id, replacement: product);
            return updateResult.IsAcknowledged 
                    && updateResult.ModifiedCount > 0;
        }
    }
}
