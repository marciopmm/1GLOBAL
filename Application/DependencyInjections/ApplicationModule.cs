using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using MM.Application.Abstractions.Services;
using MM.Application.Services.DeviceDTOs;
using MM.Application.Services.Devices;
using MM.Domain.Ports;

namespace MM.Application.DependencyInjection;

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