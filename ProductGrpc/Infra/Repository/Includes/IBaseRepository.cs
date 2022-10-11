using ProductGrpc.Models.Includes;

namespace ProductGrpc.Infra.Repository.Includes
{
    public interface IBaseRepository<T, Key> 
        where T : class
    {
        Task<T?> GetAsync(Key key);
        Task<T> UpdateAsync(T item);
        Task DeleteAsync(T item);
        Task<IQueryable<T>> GetAllAsync();
        Task<T> AddAsync(T item);
    }
}
