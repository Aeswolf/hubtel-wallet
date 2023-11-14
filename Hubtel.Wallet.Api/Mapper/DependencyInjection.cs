using Mapster;
using MapsterMapper;
using System.Reflection;

namespace Hubtel.Wallet.Api.Mapper;

public static class DependencyInjection
{
    public static IServiceCollection AddApiMapper(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;

        config.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton(config);

        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }
}
