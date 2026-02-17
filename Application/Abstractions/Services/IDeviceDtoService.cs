using MM.Application.DTOs;

namespace MM.Application.Abstractions.Services;

public interface IDeviceDtoService
{
    Task<IEnumerable<DeviceDTO>> GetAllDevicesAsync();
    Task<DeviceDTO> GetDeviceByIdAsync(Guid id);
    Task<IEnumerable<DeviceDTO>> GetDevicesByQueryAsync(string? name, string? brand, string? state);
    Task<DeviceDTO> AddDeviceAsync(AddDeviceDtoRequest deviceDto);
    Task<DeviceDTO> UpdateDeviceAsync(Guid id, UpdateDeviceDtoRequest deviceDto);
    Task<DeviceDTO> UpdateDevicePartialAsync(Guid id, UpdateDeviceDtoRequest deviceDto);
    Task DeleteDeviceAsync(Guid id);
}