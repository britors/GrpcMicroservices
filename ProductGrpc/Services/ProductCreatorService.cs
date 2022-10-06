using Grpc.Core;
using ProductGrpc.Application.Includes;
using ProductGrpc.Models;
using ProductGrpc.Models.Enums;
using ProductGrpc.Protos;

namespace ProductGrpc.Services
{
    public class ProductCreatorService : ProductCreator.ProductCreatorBase
    {
        private readonly IProductApplication _productApplication;

        public ProductCreatorService(IProductApplication productApplication)
        {
            _productApplication = productApplication;
        }

        public override async Task<ProductResponse> Create(ProductRequest request, ServerCallContext context)
        {
            var product = new Product(request.Name, request.Description, request.Price, (ProductStatus)request.Status);
            var result = await _productApplication.SaveAsync(product);

            var response = new ProductResponse
            {
                Id = result.Id.ToString(),
                Name = result.Name,
                Description = result.Description,
                Price = result.Price,
                Status = (int)result.Status,
                CreatedAt = result.CreatedAt.ToString("dd/MM/yyyy")
            };

            return response;
        }
    }
}
