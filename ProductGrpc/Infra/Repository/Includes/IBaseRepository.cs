using System.Linq.Expressions;

namespace ProductGrpc.Infra.Repository.Includes
{
    public interface IBaseRepository<T, Key> 
        where T : class
    {
        Task<T> AddAsync(T item);
        Task<T> UpdateAsync(T item, T source);
        Task DeleteAsync(T item);
        Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, string[]? includes = null);
        Task<T?> GetAsync(Key key);
    }
}
