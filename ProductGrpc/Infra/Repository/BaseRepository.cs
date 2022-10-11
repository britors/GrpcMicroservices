using Microsoft.EntityFrameworkCore;
using ProductGrpc.Data.Context;
using ProductGrpc.Infra.Repository.Includes;

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
        public async Task<T> UpdateAsync(T item)
        {
            _context.Entry(item).State = EntityState.Modified;
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
        public async Task<IQueryable<T>> GetAllAsync(){
            var items = _context.Set<T>().AsQueryable();
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
