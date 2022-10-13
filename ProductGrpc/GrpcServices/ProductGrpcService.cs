using Grpc.Core;
using ProductGrpc.Services.Includes;
using ProductGrpc.Protos;
using Google.Protobuf.WellKnownTypes;

namespace ProductGrpc.GrpcServices
{
    public class ProductGrpcService : ProductGrpcCommunicator.ProductGrpcCommunicatorBase
    {
        private readonly IProductService _productService;
        public ProductGrpcService(IProductService productService)
        => _productService = productService;

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

                var newProduct = await _productService.AddAsync<ProductModel, ProductCreateRequest>(request);

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

                var product = await _productService.UpdateAsync<ProductModel, ProductUpdateRequest>(request);

                if (product == null)
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
        /// Setar produto como Ativo (Possue Estoque)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Empty> Active(ProductIndexRequest request, ServerCallContext context)
        {
            await _productService.ChangeStatus(request, 3);
            return new Empty();
        }

        /// <summary>
        /// Setar produto como Inativo (Sem Estoque)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Empty> Inactive(ProductIndexRequest request, ServerCallContext context)
        {
            await _productService.ChangeStatus(request, 1);
            return new Empty();
        }

        /// <summary>
        /// Excluir um produto
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="RpcException"></exception>
        public override async Task<Empty> Delete(ProductIndexRequest request, ServerCallContext context)
        {
            try
            {
                await _productService.DeleteAsync(request);
                return new Empty();
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
                var model = await _productService.GetAsync<ProductModel, ProductIndexRequest>(request);

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
                var model = await _productService.GetAsync<ProductModel, ProductIndexRequest>(request);

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
                var products = await _productService.GetProducts(request);
                var result = new ProductsResult();
                foreach (var product in products)
                {
                    var model = _productService.GetReturn<ProductModel>(product);
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
                var products = await _productService.GetProducts(request);
                foreach (var product in products)
                {
                    var model = _productService.GetReturn<ProductModel>(product);
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
        /// Marcar produto como deletado
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Empty> SetAsDeleted(ProductIndexRequest request, ServerCallContext context)
        {
            await _productService.ChangeIdDeleted(request, true);
            return new Empty();
        }

        /// <summary>
        /// Desmarcar produto como deletado
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override  async Task<Empty> UndoDeleted(ProductIndexRequest request, ServerCallContext context)
        {
            await _productService.ChangeIdDeleted(request, false);
            return new Empty();
        }
    }
}
