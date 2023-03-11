using Catalog.API.Models;

namespace Catalog.API.Interfaces;

public interface IProductRepository {

   Task<IEnumerable<Product>> GetProducts();
   Task<Product> GetProduct(string id);
   Task<IEnumerable<Product>> GetProductByName(string name);
   Task<IEnumerable<Product>> GetProductByCategory(string categoryName);

   Task CreateProduct(Product product);
   Task<bool> UpdateProduct(Product product);
   Task<bool> DeleteProduct(string id);

}
