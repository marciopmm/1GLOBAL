using Global.Application.DTOs;

namespace Global.Application.Abstractions.Services;

public interface IDeviceDtoService
{
    Task<IEnumerable<DeviceDTO>> GetAllDevicesAsync();
    Task<IEnumerable<DeviceDTO>> GetDevicesByStateAsync(string state);
    Task<IEnumerable<DeviceDTO>> GetDevicesByBrandAsync(string brand);
    Task<DeviceDTO> GetDeviceByIdAsync(int id);
    Task<DeviceDTO> AddDeviceAsync(DeviceDTO deviceDto);
    Task UpdateDeviceAsync(DeviceDTO deviceDto);
    Task DeleteDeviceAsync(int id);
}