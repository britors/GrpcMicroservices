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

            return new Guid(id);

        }

        public override TResponse? GetReturn<TResponse>(Product model)
            where TResponse : class
        {
            var status = model.Status;
            var response = new ProductModel
            {
                Id = model.Id.ToString().ToUpper(),
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Status = status != null ?
                                    StatusModelToResult(status) :
                                    null,
                StatusId = model.StatusId,
                CreatedAt = Timestamp.FromDateTime(model.CreatedAt.ToUniversalTime()),
            };
            return response as TResponse;
        }

        protected override async Task<Product> GetModel<TRequest>(TRequest request, CrudType? type = null)
        {
            var id = GetKey(request);
            var name = GetValueInRequest(request, "Name") as string;
            var description = GetValueInRequest(request, "Description") as string;
            var status = GetValueInRequest(request, "StatusId") as int?;
            var price = GetValueInRequest(request, "Price") as float?;
            var isDeleted = GetValueInRequest(request, "IsDeleted") as bool?;

            DateTime createAt = new();
            DateTime? updatedAt = null;

            if (type != null)
            {
                switch (type)
                {
                    case CrudType.Insert:
                        createAt = DateTime.UtcNow;
                        isDeleted = false;
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
                        status = status == 0 ? product.StatusId : status;
                        isDeleted = isDeleted == null ? product.IsDeleted : isDeleted;
                        updatedAt = DateTime.UtcNow;
                        break;
                }
            }

            return new Product
            {
                Id = id,
                Name = name ?? "",
                Description = description ?? "",
                StatusId = status ?? (int)ProductStatusEnum.NONE,
                Price = price ?? 0,
                IsDeleted = isDeleted ?? true,
                CreatedAt = createAt,
                UpdateAt = updatedAt
            };
        }

        public async Task<IEnumerable<Product>> GetProducts(ProductFilter filter)
        {
            var filters = ReturnProductFilter(filter);

            Expression<Func<Product, bool>> predicate = (x => x.IsDeleted.Equals(false));

            if (!string.IsNullOrEmpty(filter.Name))
                predicate = FilterHelper<Product>.UnionWithAnd(predicate, filters["Name"]);

            if (filter.StatusId > 0)
                predicate = FilterHelper<Product>.UnionWithAnd(predicate, filters["StatusId"]);


            return await GetAllAsync(predicate, new[] { "Status" });
        }

        private static Dictionary<string, Expression<Func<Product, bool>>>
            ReturnProductFilter(ProductFilter filter)
        {
            var queries = new Dictionary<string, Expression<Func<Product, bool>>>
            {
                { "Name", (x => x.Name == filter.Name) },
                { "StatusId", (x => x.StatusId.Equals(filter.StatusId)) },
            };

            return queries;
        }

        private static StatusResult StatusModelToResult(ProductStatus status)
            => new()
            {
                Id = status.Id.ToString(),
                Name = status.Name
            };

    }
}
