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
        public async Task<Product> GetByIdAsync(Guid id)
        {
            var product = await _repository.GetAsync(id);
            if (product == null)
                throw new ArgumentException("Produto não encontrado");

            return product;
        }
        public async Task<Product> SaveAsync(Product product, bool IsNewProduct = false)
        {
            if (IsNewProduct)
                return await _repository.AddAsync(product);
            else
                return await _repository.UpdateAsync(product);
        }
    }
}
