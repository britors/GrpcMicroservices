using ProductGrpc.Models.Includes;

namespace ProductGrpc.Infra.Repository.Includes
{
    public interface IBaseRepository<T, Key> 
        where T : class
    {
        Task<T> AddAsync(T item);
        Task<T> UpdateAsync(T item);
        Task DeleteAsync(T item);
        Task<IQueryable<T>> GetAllAsync(Func<T, bool>? predicate = null, string[]? includes = null);
        Task<T?> GetAsync(Key key);
    }
}
