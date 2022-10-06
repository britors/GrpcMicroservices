using ProductGrpc.Models;

namespace ProductGrpc.Infra.Repository.Includes
{
    public interface IProductRepository
    {
        Task<Product> SaveAsync(Product product);
        Task<Product?> GetByIdAsync(Guid id);
        Task<IQueryable<Product>> GetAllAsync();
    }
}
