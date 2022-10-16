using ProductGrpc.GrpcServices;

namespace ProductGrpc.CrossCutting.DI
{
    public static class GrpcServices
    {
        public static WebApplication MapGrpcServices(this WebApplication app)
        {
            app.MapGrpcService<ProductGrpcService>();
            app.MapGrpcService<ProductStatusGrpcService>();
            return app;
        }
    }
}
