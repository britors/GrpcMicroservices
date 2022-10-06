using ProductGrpc.Application.Includes;
using ProductGrpc.Infra.Repository.Includes;
using ProductGrpc.Models;

namespace ProductGrpc.Application
{
    public class ProductApplication : IProductApplication
    {

        private readonly IProductRepository _repository;

        public ProductApplication(IProductRepository repository) => _repository = repository;
        public async Task<IEnumerable<Product>> GetAllAsync() => await _repository.GetAllAsync();
        public async Task<Product?> GetByIdAsync(Guid id) => await _repository.GetByIdAsync(id);
        public async Task<Product> SaveAsync(Product product) => await _repository.SaveAsync(product);
    }
}
