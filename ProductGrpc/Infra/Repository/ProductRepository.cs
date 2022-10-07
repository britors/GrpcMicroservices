using Microsoft.EntityFrameworkCore;
using ProductGrpc.Data.Context;
using ProductGrpc.Infra.Repository.Includes;
using ProductGrpc.Models;

namespace ProductGrpc.Infra.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductContext _context;

        public ProductRepository(ProductContext context) =>
            _context = context;

        public async Task<Product> AddAsync(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return await Task.FromResult(product);
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            _context.SaveChanges();
            return await Task.FromResult(product);
        }

        public async Task<IQueryable<Product>> GetAllAsync() =>
            await Task.FromResult(_context.Products);

        public async Task<Product?> GetByIdAsync(Guid id) =>
            await _context
                    .Set<Product>()
                    .FindAsync(id);
    }
}
