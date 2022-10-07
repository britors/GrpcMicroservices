namespace ProductGrpc.Infra.Repository.Includes
{
    public interface IBaseRepository<T, Key> where T : class
    {
        Task<T?> GetAsync(Key key);
        Task<IQueryable<T>> GetAllAsync();
        Task<T> AddAsync(T item);
        Task<T> UpdateAsync(T item);
    }
}
