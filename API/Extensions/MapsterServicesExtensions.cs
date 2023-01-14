using System.Reflection;
using Mapster;
using MapsterMapper;

namespace API.Extensions
{
    public static class MapsterServicesExtensions
    {
        public static IServiceCollection AddMapsterServices(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());

            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();

            return services;
        }
    }
}