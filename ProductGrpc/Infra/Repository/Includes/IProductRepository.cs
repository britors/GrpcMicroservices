using ProductGrpc.Models;

namespace ProductGrpc.Infra.Repository.Includes
{
    public interface IProductRepository: IBaseRepository<Product, Guid>
    {
    }
}
