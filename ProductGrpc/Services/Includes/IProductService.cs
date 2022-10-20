using ProductGrpc.Models;
using ProductGrpc.Protos;

namespace ProductGrpc.Services.Includes
{
    public interface IProductService : IBaseService<Product>
    {
        Task<IEnumerable<Product>> GetProducts(ProductFilter filter);
        Task ChangeStatus(ProductIndexRequest request, short statusId);
        Task ChangeDeletedState(ProductIndexRequest request, bool isDeleted);
    }
}
