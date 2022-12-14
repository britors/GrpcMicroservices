using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using ProductGrpc.Protos;
using ProductGrpc.Services.Includes;

namespace ProductGrpc.GrpcServices
{
    public class ProductStatusGrpcService : ProductStatusGrpcComunicator.ProductStatusGrpcComunicatorBase
    {
        private readonly IProductStatusService _productStatusService;
        public ProductStatusGrpcService(IProductStatusService productStatusService)
        {
            _productStatusService = productStatusService;
        }

        /// <summary>
        /// Retornar os Status possiveis de um pedido
        /// </summary>
        /// <param name="request"></param>
        /// <param name="responseStream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task GetProductStatusesAsync(Empty request, IServerStreamWriter<ProductStatusResult> responseStream, ServerCallContext context)
        {
            var statuses = await _productStatusService.GetAllAsync();
            foreach (var status in statuses)
            {
                if (!status.IsDeleted)
                {
                    var result = _productStatusService.GetReturn<ProductStatusResult>(status);
                    if (result is not null)
                    {
                        await responseStream.WriteAsync(result);
                    }
                }
            }
        }
    }
}


