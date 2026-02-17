using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MM.Infrastructure.Persistence.Db;

using MM.Domain.Ports;
using MM.Infrastructure.Persistence.Db;
using MM.Infrastructure.Persistence.Repository;
using MM.Infrastructure.Persistence.Abstractions;

namespace MM.Infrastructure.Persistence.DependencyInjection;

public static class PersistenceModule
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // DbContext
        services.AddDbContext<MMDbContext>();

        // Repositories (Ports and Adapters Implementations)
        services.AddScoped<IMMDbContext>(sp => sp.GetRequiredService<MMDbContext>());
        services.AddScoped<IDeviceRepository, DeviceRepository>();

        return services;
    }
}