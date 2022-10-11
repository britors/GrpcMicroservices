using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using ProductGrpc.Application.Includes;
using ProductGrpc.Infra.Repository.Includes;
using ProductGrpc.Models;
using ProductGrpc.Models.Enums;
using ProductGrpc.Protos;

namespace ProductGrpc.Services
{
    public class ProductService : ProductGrpcCommunicator.ProductGrpcCommunicatorBase
    {
        private readonly IProductApplication _productApplication;
        public ProductService(IProductApplication productApplication)
        => _productApplication = productApplication;



        /// <summary>
        /// Criar novo Produto
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="RpcException"></exception>
        public override async Task<ProductModel> Create(ProductCreateRequest request, ServerCallContext context)
        {
            try
            {

                var newProduct = await _productApplication.AddAsync<ProductModel, ProductCreateRequest>(request);

                if (newProduct == null)
                    throw new RpcException(new Status(StatusCode.Internal, "Erro ao cadastrar o produto"));

                return newProduct;
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
        public override async Task<ProductModel> Update(ProductUpdateRequest request, ServerCallContext context)
        {
            try
            {

                var product = await _productApplication.UpdateAsync<ProductModel, ProductUpdateRequest>(request);
                
                if(product == null)
                    throw new RpcException(new Status(StatusCode.Internal, "Erro ao cadastrar o produto"));

                return product;
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
                await _productApplication.DeleteAsync(request);
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
        public override async Task<ProductModel> GetProduct(ProductIndexRequest request, ServerCallContext context)
        {
            try
            {
                var model = await _productApplication.GetAsync<ProductModel, ProductIndexRequest>(request);

                if (model == null)
                    throw new RpcException(new Status(StatusCode.Internal, "Product não encontado"));

                return model;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new RpcException(new Status(StatusCode.Internal, e.Message));
            }
        }

        /// <summary>
        /// Buscar produto (async)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="responseStream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="RpcException"></exception>
        public override async Task GetProductAsync(ProductIndexRequest request,
                                                    IServerStreamWriter<ProductModel> responseStream,
                                                    ServerCallContext context)
        {
            try
            {
                var model = await _productApplication.GetAsync<ProductModel, ProductIndexRequest>(request);

                if (model == null)
                    throw new RpcException(new Status(StatusCode.Internal, "Product não encontado"));

                await responseStream.WriteAsync(model);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new RpcException(new Status(StatusCode.Internal, e.Message));
            }

        }

        /// <summary>
        /// Retornar lista de produtos (sync)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="RpcException"></exception>
        public override async Task<ProductsResult> GetProducts(ProductFilter request, ServerCallContext context)
        {
            try
            {
                var products = await GetProducts(request);
                var result = new ProductsResult();
                foreach (var product in products)
                {
                    var model = _productApplication.GetReturn<ProductModel>(product);
                    result.Products.Add(model);
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
        /// Retorna uma lista de produtos (async)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        /// 
        public override async Task GetProductsAsync(ProductFilter request,
                                                    IServerStreamWriter<ProductModel> responseStream,
                                                    ServerCallContext context)
        {
            try
            {
                var products = await GetProducts(request);
                foreach (var product in products)
                {
                    var model = _productApplication.GetReturn<ProductModel>(product);
                    if (model != null)
                        await responseStream.WriteAsync(model);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new RpcException(new Status(StatusCode.Internal, e.Message));
            }
        }


        /// <summary>
        /// Buscar produto
        /// </summary>
        /// <returns></returns>
        private async Task<IEnumerable<Product>> GetProducts(ProductFilter filter)
        {
            Func<Product, bool>? predicate = null;
            
            // TODO : Melhorar filtro
            if (!string.IsNullOrEmpty(filter.Name))
                predicate = (x => x.Name == filter.Name);

            if (filter.Status > 0)
                predicate = predicate == null ?  
                    (x => x.Status.Equals((ProductStatus)filter.Status)) 
                    : (x => x.Name == filter.Name && x.Status.Equals((ProductStatus)filter.Status));

            var products = await _productApplication.GetAllAsync(predicate);
            return products;
        }

    }
}
