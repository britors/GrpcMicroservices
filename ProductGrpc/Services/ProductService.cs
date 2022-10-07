using Grpc.Core;
using ProductGrpc.Application.Includes;
using ProductGrpc.Models;
using ProductGrpc.Models.Enums;
using ProductGrpc.Protos;

namespace ProductGrpc.Services
{
    public class ProductService : ProductGrpcCommunicator.ProductGrpcCommunicatorBase
    {
        private readonly IProductApplication _productApplication;

        public ProductService(IProductApplication productApplication)
        {
            _productApplication = productApplication;
        }

        public override async Task<ProductResult> Create(ProductCreateRequest request, ServerCallContext context)
        {
            try
            {

                var product = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Description = request.Description,
                    Price = request.Price,
                    Status = ProductStatus.NONE,
                    CreatedAt = DateTime.UtcNow,
                };

                var result = await _productApplication.SaveAsync(product, true);
                return BuildReturn(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new RpcException(new Status(StatusCode.Internal, e.Message));
            }
        }

        public override async Task<ProductResult> Update(ProductUpdateRequest request, ServerCallContext context)
        {
            try
            {
                var productId = new Guid(request.Id);
                var product = await _productApplication.GetByIdAsync(productId);
                product.Name = request.Name;
                product.Description = request.Description;
                product.Price = request.Price;
                var result = await _productApplication.SaveAsync(product);
                return BuildReturn(result);
            }
            catch (KeyNotFoundException e)
            {
                Console.WriteLine(e);
                throw new RpcException(new Status(StatusCode.InvalidArgument, e.Message));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new RpcException(new Status(StatusCode.Internal, e.Message));
            }
        }

        public override async Task<ProductDeleted> Delete(ProductIndexRequest request, ServerCallContext context)
        {
            try
            {
                var productId = new Guid(request.Id);
                var product = await _productApplication.GetByIdAsync(productId);
                await _productApplication.DeletedAsync(product);
                return new ProductDeleted
                {
                    Id = request.Id
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new RpcException(new Status(StatusCode.Internal, e.Message));
            }
        }

        public override async Task<ProductResult> GetProduct(ProductIndexRequest request, ServerCallContext context)
        {
            try
            {
                var productId = new Guid(request.Id);
                var product = await _productApplication.GetByIdAsync(productId);
                return BuildReturn(product);
            }
            catch (KeyNotFoundException e)
            {
                Console.WriteLine(e);
                throw new RpcException(new Status(StatusCode.InvalidArgument, e.Message));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new RpcException(new Status(StatusCode.Internal, e.Message));
            }
        }

        /// <summary>
        /// Cria um retorno da chamada
        /// </summary>
        /// <param name="product">Produto</param>
        /// <returns>Result</returns>
        private static ProductResult BuildReturn(Product product)
        {
            return new ProductResult
            {
                Id = product.Id.ToString(),
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Status = (Int32)product.Status,
                CreatedAt = product.CreatedAt.ToString("dd/MM/yyyy")
            };
        }
    }
}
