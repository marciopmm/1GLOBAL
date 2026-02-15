using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Global.Application.Abstractions.Services.DeviceDTOs;
using Global.Application.Services.DeviceDTOs;
using Global.Application.Services.Devices;
using Global.Domain.Ports;

namespace Global.Application.DependencyInjection;

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