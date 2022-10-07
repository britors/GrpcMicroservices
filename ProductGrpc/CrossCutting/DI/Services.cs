using ProductGrpc.Services;

namespace ProductGrpc.CrossCutting.DI
{
    public static class Services
    {
        public static WebApplication MapGrpcServices(this WebApplication app)
        {
            app.MapGrpcService<ProductService>();
            return app;
        }
    }
}
