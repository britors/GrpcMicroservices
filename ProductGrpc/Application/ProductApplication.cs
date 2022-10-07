using ProductGrpc.Application.Includes;
using ProductGrpc.Infra.Repository.Includes;
using ProductGrpc.Models;

namespace ProductGrpc.Application
{
    public class ProductApplication : IProductApplication
    {

        private readonly IProductRepository _repository;
        public ProductApplication(IProductRepository repository) => _repository = repository;

        /// <summary>
        /// Excluir um item
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task DeletedAsync(Product product) => await _repository.DeleteAsync(product);

        /// <summary>
        /// Buscar todos os registros
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Product>> GetAllAsync() => await _repository.GetAllAsync();

        /// <summary>
        /// Buscar registro pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<Product> GetByIdAsync(Guid id)
        {
            var product = await _repository.GetAsync(id);
            if (product == null)
                throw new KeyNotFoundException("Produto não encontrado");

            return product;
        }
        
        /// <summary>
        /// Salvar um registro no banco
        /// </summary>
        /// <param name="product">Produto para salvar</param>
        /// <param name="IsNewProduct">Eh registro novo?</param>
        /// <returns></returns>
        public async Task<Product> SaveAsync(Product product, bool IsNewProduct = false)
        {
            if (IsNewProduct)
                return await _repository.AddAsync(product);
            else
                return await _repository.UpdateAsync(product);
        }
    }
}
