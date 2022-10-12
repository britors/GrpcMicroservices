using Microsoft.EntityFrameworkCore;
using ProductGrpc.Data.Context;
using ProductGrpc.Infra.Repository.Includes;
using System.Linq.Expressions;

namespace ProductGrpc.Infra.Repository
{
    public class BaseRepository<T, Key> : IBaseRepository<T, Key>
        where T : class
    {
        private readonly ProductContext _context;

        public BaseRepository(ProductContext context) =>
            _context = context;

        /// <summary>
        /// Criar registro
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<T> AddAsync(T item)
        {
            _context.Entry(item).State = EntityState.Added;
            _context.Add(item);
            await _context.SaveChangesAsync();
            return await Task.FromResult(item);
        }

        /// <summary>
        /// Atualizar o registro
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<T> UpdateAsync(T item, T source)
        {
            _context.Entry(source).CurrentValues.SetValues(item);
            await _context.SaveChangesAsync();
            return await Task.FromResult(item);
        }

        /// <summary>
        /// Excluir um registro da entidade
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task DeleteAsync(T item)
        {
            _context.Entry(item).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Retornar todos os registros da entidade
        /// </summary>
        /// <returns></returns>
        public async Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null,   
                                                                    string[]? includes = null)
        {
            var items = predicate != null 
                ? _context.Set<T>().Where(predicate).AsQueryable() 
                : _context.Set<T>().AsQueryable();

            if (includes != null && includes.Length > 0)
                foreach (var include in includes)
                    items = items.Include(include);

            return await Task.FromResult(items);
        }

        /// <summary>
        /// Buscar registro pela chave primaria
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T?> GetAsync(Key key)
            => await _context
                    .Set<T>()
                    .FindAsync(key);
    }
}
