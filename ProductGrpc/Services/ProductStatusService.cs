using ProductGrpc.Services.Includes;
using ProductGrpc.Infra.Repository.Includes;
using ProductGrpc.Models;
using ProductGrpc.Protos;

namespace ProductGrpc.Services
{
    public class ProductStatusService : BaseService<ProductStatus, int>, IProductStatusService
    {
        
        public ProductStatusService(IProductStatusRepository productStatusRepository) 
            : base(productStatusRepository) { }

        protected override int GetKey<TRequest>(TRequest request)
        {
            var id = GetValueInRequest(request, "Id") as int?;
            return id ?? 0;

        }

        public override TResponse? GetReturn<TResponse>(ProductStatus model)
            where TResponse : class
        {
            return new ProductStatusResult
            {
                Id = model.Id,
               Name = model.Name,
            } as TResponse;
        }
    }
}
