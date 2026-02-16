using OneGlobal.Application.DTOs;

namespace OneGlobal.Application.Abstractions.Services;

public interface IDeviceDtoService
{
    Task<IEnumerable<DeviceDTO>> GetAllDevicesAsync();
    Task<IEnumerable<DeviceDTO>> GetDevicesByStateAsync(string state);
    Task<IEnumerable<DeviceDTO>> GetDevicesByBrandAsync(string brand);
    Task<DeviceDTO> GetDeviceByIdAsync(Guid id);
    Task<DeviceDTO> AddDeviceAsync(AddDeviceDtoRequest deviceDto);
    Task<DeviceDTO> UpdateDeviceAsync(Guid id, UpdateDeviceDtoRequest deviceDto);
    Task<DeviceDTO> UpdateDevicePartialAsync(Guid id, UpdateDeviceDtoRequest deviceDto);
    Task DeleteDeviceAsync(Guid id);
}