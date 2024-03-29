using Catalog.API.Data;
using Catalog.API.Models;
using Catalog.API.Interfaces;
using MongoDB.Driver;

namespace Catalog.API.Repositories;

public class ProductRepository : IProductRepository
{
   private readonly ICatalogContext _context;

   public ProductRepository(ICatalogContext context)
   {
      _context = context ?? throw new ArgumentNullException(nameof(context));
   }

   public async Task CreateProduct(Product product)
   {
      await _context.Products.InsertOneAsync(product);
   }

   public async Task<bool> DeleteProduct(string id)
   {
      var filter = Builders<Product>.Filter.Eq(m => m.Id, id);
      var deleteResult = await _context.Products.DeleteOneAsync(filter);

      return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
   }

   public async Task<Product> GetProduct(string id)
   {
      return await _context.Products.Find(_ => _.Id == id).FirstOrDefaultAsync();
   }

   public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
   {
      var filter = Builders<Product>.Filter.Eq(p => p.Category, categoryName);
      return await _context.Products.Find(filter).ToListAsync();
   }

   public async Task<IEnumerable<Product>> GetProductByName(string name)
   {
      var filter = Builders<Product>.Filter.Eq(p => p.Name, name);
      return await _context.Products.Find(filter).ToListAsync();
   }

   public async Task<IEnumerable<Product>> GetProducts()
   {
      return await _context.Products.Find(_ => true).ToListAsync();
   }

   public async Task<bool> UpdateProduct(Product product)
   {
      var updateResult = await _context.Products.ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);

      return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
   }
}
