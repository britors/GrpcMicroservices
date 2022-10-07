using ProductGrpc.Models;

namespace ProductGrpc.Application.Includes
{
    public interface IProductApplication
    {
        Task<Product> SaveAsync(Product product, bool IsNewProduct = false);
        Task DeletedAsync(Product product);
        Task<Product> GetByIdAsync(Guid id);
        Task<IEnumerable<Product>> GetAllAsync();
    }
}
