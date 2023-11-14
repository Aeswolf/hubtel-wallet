using Hubtel.Wallet.Api.Interfaces;
using Hubtel.Wallet.Api.Mapper;
using Hubtel.Wallet.Api.Persistence;
using Hubtel.Wallet.Api.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hubtel.Wallet.Api.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        var postgresStore = configuration.GetConnectionString("PostgresStore");

        services.AddDbContext<WalletDbContext>(options => options.UseNpgsql(postgresStore));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddApiMapper();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection)));

        services.AddControllers();

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen();

        return services;
    }
}
