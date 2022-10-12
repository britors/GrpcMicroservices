using System.Linq.Expressions;

namespace ProductGrpc.Services.Includes
{
    public interface IBaseService<TModel>
        where TModel : class
    {
        Task<TResponse?> AddAsync<TResponse,TRequest>(TRequest request)
            where TResponse : class
            where TRequest: class;

        Task<TResponse?> UpdateAsync<TResponse, TRequest>(TRequest request)
            where TResponse : class
            where TRequest : class;

        Task DeleteAsync<TRequest>(TRequest request) 
            where TRequest : class;

        Task<TResponse?> GetAsync<TResponse, TRequest>(TRequest request)
            where TResponse : class
            where TRequest : class;

        Task<IQueryable<TModel>> GetAllAsync(Expression<Func<TModel, bool>>? predicate = null, string[]? includes = null);

        TResponse? GetReturn<TResponse>(TModel model) where TResponse : class;
    }
}
