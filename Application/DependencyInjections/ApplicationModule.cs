using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using OneGlobal.Application.Abstractions.Services;
using OneGlobal.Application.Services.DeviceDTOs;
using OneGlobal.Application.Services.Devices;
using OneGlobal.Domain.Ports;

namespace OneGlobal.Application.DependencyInjection;

public static class ApplicationModule
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<IDeviceService, DeviceService>();
        services.AddScoped<IDeviceDtoService, DeviceDtoService>();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }
}