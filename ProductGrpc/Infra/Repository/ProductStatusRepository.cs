using ProductGrpc.Data.Context;
using ProductGrpc.Infra.Repository.Includes;
using ProductGrpc.Models;

namespace ProductGrpc.Infra.Repository
{
    public class ProductStatusRepository : BaseRepository<ProductStatus, int>, IProductStatusRepository
    {
        public ProductStatusRepository(ProductContext context) : base(context) { }
    }
}
