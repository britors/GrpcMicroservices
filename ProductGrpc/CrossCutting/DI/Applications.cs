using ProductGrpc.Application;
using ProductGrpc.Application.Includes;

namespace ProductGrpc.CrossCutting.DI
{
    public static class Applications
    {
        public static IServiceCollection MakeInjectDependencies(this IServiceCollection collection)
        {
            collection.AddScoped<IProductApplication, ProductApplication>();
            return collection;
        }
    }
}
