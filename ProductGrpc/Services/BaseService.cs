using ProductGrpc.Services.Includes;
using ProductGrpc.Infra.Repository.Includes;
using System.Linq.Expressions;

namespace ProductGrpc.Services
{
    public abstract class BaseService<TModel, TKey> : IBaseService<TModel>
        where TModel : class
    {
        private readonly IBaseRepository<TModel, TKey> _baseRepository;
        protected BaseService(IBaseRepository<TModel, TKey> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async Task<TResponse?> AddAsync<TResponse, TRequest>(TRequest request)
            where TRequest : class
            where TResponse : class
        {
            var model = await GetModel(request, CrudType.Insert);
            var result = await _baseRepository.AddAsync(model);
            return GetReturn<TResponse>(result);
        }

        public async Task<TResponse?> UpdateAsync<TResponse, TRequest>(TRequest request)
            where TRequest : class
            where TResponse : class
        {
            var model = await GetModel(request, CrudType.Update);
            var key = GetKey(request);
            var source = await _baseRepository.GetAsync(key);
            if (source == null)
            {
                Exception exception = new("Produto não encontrado");
                throw exception;
            }

            var result = await _baseRepository.UpdateAsync(model, source);
            return GetReturn<TResponse>(result);
        }

        public async Task DeleteAsync<TRequest>(TRequest request) where TRequest : class
        {
            var key = GetKey(request);
            var model = await _baseRepository.GetAsync(key);
            if (model == null)
                throw new ArgumentNullException(GetType().Name);
            await _baseRepository.DeleteAsync(model);
        }

        public async Task<IQueryable<TModel>> GetAllAsync(Expression<Func<TModel, bool>>? predicate = null,
                                                                                   string[]? includes = null)
            => await _baseRepository.GetAllAsync(predicate, includes);

        public async Task<TResponse?> GetAsync<TResponse, TRequest>(TRequest request)
            where TResponse : class
            where TRequest : class
        {
            var key = GetKey(request);
            var model = await _baseRepository.GetAsync(key);
            if (model == null)
                throw new ArgumentNullException(GetType().Name);

            return GetReturn<TResponse>(model);
        }

        protected object? GetValueInRequest<TRequest>(TRequest request, string propertyName)
        {
            if (request == null)
                return null;

            var propertyInfo = request.GetType().GetProperty(propertyName);

            if (propertyInfo == null)
                return null;

            var value = propertyInfo.GetValue(request, null);
            return value;
        }

        protected virtual TKey GetKey<TRequest>(TRequest request) where TRequest : class
            => throw new NotImplementedException("Metodo GetKey não implementado");

        protected virtual Task<TModel> GetModel<TRequest>(TRequest request, CrudType? type = null) 
            where TRequest : class
            => throw new NotImplementedException("Metodo GetModel não implementado");

        public virtual TResponse? GetReturn<TResponse>(TModel model) where TResponse : class
            => throw new NotImplementedException("Metodo GetReturn não implementado");

    }

    public enum CrudType
    {
        Insert,
        Update,
    }
}
