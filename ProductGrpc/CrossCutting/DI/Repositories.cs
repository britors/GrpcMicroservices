using ProductGrpc.Infra.Repository.Includes;
using ProductGrpc.Infra.Repository;

namespace ProductGrpc.CrossCutting.DI
{
    public static class Repositories
    {
        public static IServiceCollection MakeInjectDependencies(this IServiceCollection collection)
        {
            collection.AddScoped<IProductRepository, ProductRepository>();
            return collection;
        }
    }
}
