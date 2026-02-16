using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OneGlobal.Infrastructure.Persistence.Db;

using OneGlobal.Domain.Ports;
using OneGlobal.Infrastructure.Persistence.Repository;

namespace OneGlobal.Infrastructure.Persistence.DependencyInjection;

public static class PersistenceModule
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // DbContext
        services.AddDbContext<GlobalDbContext>();

        // Repositories (implementações das PORTS)
        services.AddScoped<IDeviceRepository, DeviceRepository>();

        return services;
    }
}