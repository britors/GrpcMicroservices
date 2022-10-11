using Google.Protobuf.WellKnownTypes;
using ProductGrpc.Application.Includes;
using ProductGrpc.Infra.Repository.Includes;
using ProductGrpc.Models;
using ProductGrpc.Models.Enums;
using ProductGrpc.Protos;

namespace ProductGrpc.Application
{
    public class ProductApplication : BaseApplication<Product, Guid>, IProductApplication
    {
        public ProductApplication(IProductRepository productRepository) : base(productRepository) { }

        protected override Guid GetKey<TRequest>(TRequest request)
        {
            var id = GetValueInRequest(request, "Id") as string;

            if (string.IsNullOrEmpty(id))
                return Guid.Empty;
            else
                return new Guid(id);

        }

        public override TResponse? GetReturn<TResponse>(Product model)
            where TResponse : class
        {
            var response = new ProductModel
            {
                Id = model.Id.ToString().ToUpper(),
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Status = (int)model.Status,
                CreatedAt = Timestamp.FromDateTime(model.CreatedAt.ToUniversalTime()),
            };
            return response as TResponse;
        }

        protected override Product GetModel<TRequest>(TRequest request)
        {
            var id = GetKey(request);
            var name = GetValueInRequest(request, "Name") as string;
            var description = GetValueInRequest(request, "Description") as string;
            var status = GetValueInRequest(request, "Status") as string;
            var price = GetValueInRequest(request, "Price") as string;
            var createAt = GetValueInRequest(request, "CreatedAt") as string;

            return new Product
            {
                Id = id,
                Name = name ?? "",
                Description = description ?? "",
                Status = status != null ? (ProductStatus)Convert.ToInt32(status) : ProductStatus.NONE,
                Price = price != null ? float.Parse(price) : 0,
                CreatedAt = createAt != null ? Convert.ToDateTime(GetValueInRequest(request, "CreatedAt")) : DateTime.UtcNow
            };
        }
    }
}
