using estore.MicroFrontEnd.Products.Models;

namespace estore.MicroFrontEnd.Products.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int? id);
        Task<Product> CreateOrUpdateProductAsync(Product product);
        Task DeleteProductAsync(int? id);
    }
}
