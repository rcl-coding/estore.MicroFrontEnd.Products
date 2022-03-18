using estore.MicroFrontEnd.Products.Options;
using estore.MicroFrontEnd.Products.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ProductsFrontendExtension
    {
        public static IServiceCollection AddProductsFrontEnd(this IServiceCollection services, Action<ApiOptions> setupAction)
        {
            services.AddTransient<IProductService, ProductService>();
            services.Configure(setupAction);
            return services;
        }
    }
}
