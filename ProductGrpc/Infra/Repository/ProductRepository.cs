using ProductGrpc.Data.Context;
using ProductGrpc.Infra.Repository.Includes;
using ProductGrpc.Models;

namespace ProductGrpc.Infra.Repository
{
    public class ProductRepository :  BaseRepository<Product, Guid>, IProductRepository
    {
        public ProductRepository(ProductContext context) : base(context) {
        }
    }
}
