using Microsoft.EntityFrameworkCore;
using ProductGrpc.Data.Context;
using ProductGrpc.Infra.Repository.Includes;
using ProductGrpc.Models;

namespace ProductGrpc.Infra.Repository
{
    public class ProductRepository :  BaseRepository<Product, Guid>, IProductRepository
    {
        private readonly ProductContext _context;

        public ProductRepository(ProductContext context) : base(context) {
            _context = context;
        }
    }
}
