using ProductGrpc.Models;
using ProductGrpc.Protos;

namespace ProductGrpc.Services.Includes
{
    public interface IProductService : IBaseService<Product>
    {
        Task<IEnumerable<Product>> GetProducts(ProductFilter filter);
        Task ChangeStatus(ProductIndexRequest request, int statusId);
        Task ChangeIdDeleted(ProductIndexRequest request, bool isDeleted);
    }
}
