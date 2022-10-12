using Google.Protobuf.WellKnownTypes;
using ProductGrpc.Services.Includes;
using ProductGrpc.Infra.Repository.Includes;
using ProductGrpc.Models;
using ProductGrpc.Models.Enums;
using ProductGrpc.Protos;
using ProductGrpc.Helpers;
using System.Linq.Expressions;

namespace ProductGrpc.Services
{
    public class ProductService : BaseService<Product, Guid>, IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository) : base(productRepository)
        {
            _productRepository = productRepository;
        }

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

        protected override async Task<Product> GetModel<TRequest>(TRequest request, CrudType? type = null)
        {
            var id = GetKey(request);
            var name = GetValueInRequest(request, "Name") as string;
            var description = GetValueInRequest(request, "Description") as string;
            var status = GetValueInRequest(request, "Status") as int?;
            var price = GetValueInRequest(request, "Price") as float?;

            DateTime createAt = new();
            DateTime? updatedAt = null;

            if (type != null)
            {
                switch (type)
                {
                    case CrudType.Insert:
                        createAt = DateTime.UtcNow;
                        updatedAt = null;
                        break;
                    case CrudType.Update:
                        var product = await _productRepository.GetAsync(id);
                        
                        if (product == null)
                        {
                            Exception exception = new("Produto não encontrado");
                            throw exception;
                        }

                        createAt = product.CreatedAt;
                        updatedAt = DateTime.UtcNow;
                        break;
                }
            }

            return new Product
            {
                Id = id,
                Name = name ?? "",
                Description = description ?? "",
                Status = status != null ? (ProductStatus)status : ProductStatus.NONE,
                Price = price ?? 0,
                CreatedAt = createAt,
                UpdateAt = updatedAt
            };
        }

        public async Task<IEnumerable<Product>> GetProducts(ProductFilter filter)
        {
            var query = MakeProductQueries(filter);


            Expression<Func<Product, bool>>? predicate = null;

            if (!string.IsNullOrEmpty(filter.Name))
                predicate = query["Name"];

            if (filter.Status > 0)
                predicate = predicate == null
                                ? query["Status"]
                                : FilterHelper<Product>.UnionWithAnd(predicate, query["Status"]);


            return await GetAllAsync(predicate);
        }

        private static Dictionary<string, Expression<Func<Product, bool>>> MakeProductQueries(ProductFilter filter)
        {
            var queries = new Dictionary<string, Expression<Func<Product, bool>>>
            {
                { "Name", (x => x.Name == filter.Name) },
                { "Status", (x => x.Status.Equals((ProductStatus)filter.Status)) },
            };

            return queries;
        }
    }
}
