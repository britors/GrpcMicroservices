using ProductGrpc.Models;

namespace ProductGrpc.Infra.Repository.Includes
{
    public interface IProductRepository
    {
        Task<Product> AddAsync(Product product);
        Task<Product> UpdateAsync(Product product);
        Task<Product?> GetByIdAsync(Guid id);
        Task<IQueryable<Product>> GetAllAsync();
    }
}
