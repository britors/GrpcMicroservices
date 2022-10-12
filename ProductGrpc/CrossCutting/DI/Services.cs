using ProductGrpc.Services;
using ProductGrpc.Services.Includes;

namespace ProductGrpc.CrossCutting.DI
{
    public static class Services
    {
        public static IServiceCollection MakeInjectDependencies(this IServiceCollection collection)
        {
            collection.AddScoped<IProductService, ProductService>();
            return collection;
        }
    }
}
