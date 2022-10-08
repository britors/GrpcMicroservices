using Grpc.Core;
using ProductGrpc.Infra.Repository.Includes;
using ProductGrpc.Models;
using ProductGrpc.Models.Enums;
using ProductGrpc.Protos;

namespace ProductGrpc.Services
{
    public class ProductService : ProductGrpcCommunicator.ProductGrpcCommunicatorBase
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
            => _productRepository = productRepository;


        /// <summary>
        /// Criar novo Produto
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="RpcException"></exception>
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

                var result = await _productRepository.AddAsync(product);
                return BuildReturn(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new RpcException(new Status(StatusCode.Internal, e.Message));
            }
        }

        /// <summary>
        /// Atualizar Produto
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="RpcException"></exception>
        public override async Task<ProductResult> Update(ProductUpdateRequest request, ServerCallContext context)
        {
            try
            {
                var productId = new Guid(request.Id);
                var product = await _productRepository.GetAsync(productId);
                if (product == null)
                {
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "Produto não encontrado"));
                }
                product.Name = request.Name;
                product.Description = request.Description;
                product.Price = request.Price;
                var result = await _productRepository.UpdateAsync(product);
                return BuildReturn(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new RpcException(new Status(StatusCode.Internal, e.Message));
            }
        }

        /// <summary>
        /// Excluir um produto
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="RpcException"></exception>
        public override async Task<ProductDeleted> Delete(ProductIndexRequest request, ServerCallContext context)
        {
            try
            {
                var productId = new Guid(request.Id);
                var product = await _productRepository.GetAsync(productId);
                if (product == null)
                {
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "Produto não encontrado"));
                }
                await _productRepository.DeleteAsync(product);
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

        /// <summary>
        /// Buscar produto pela chave primaria
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="RpcException"></exception>
        public override async Task<ProductResult> GetProduct(ProductIndexRequest request, ServerCallContext context)
        {
            try
            {
                var productId = new Guid(request.Id);
                var product = await _productRepository.GetAsync(productId);
                if (product == null)
                {
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "Produto não encontrado"));
                }
                return BuildReturn(product);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new RpcException(new Status(StatusCode.Internal, e.Message));
            }
        }

        /// <summary>
        /// Retorna uma lista de produtos
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<ProductsResult> GetProducts(ProductFiltersRequest request, ServerCallContext context)
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                var result = new ProductsResult();
                foreach (var product in products)
                {
                    result.Products.Add(BuildReturn(product));
                }
                return result;
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
