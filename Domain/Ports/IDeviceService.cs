using OneGlobal.Domain.Entities;
using OneGlobal.Domain.Enums;

namespace OneGlobal.Domain.Ports
{
    public interface IDeviceService
    {
        Task<IEnumerable<Device>> GetAllDevicesAsync();
        Task<IEnumerable<Device>> GetDevicesByStateAsync(State state);
        Task<IEnumerable<Device>> GetDevicesByBrandAsync(string brand);
        Task<Device> GetDeviceByIdAsync(Guid id);
        Task<Device> AddDeviceAsync(Device device);
        Task<Device> UpdateDeviceAsync(Guid id, DevicePatch devicePatch);
        Task<Device> UpdateDevicePartialAsync(Guid id, DevicePatch devicePatch);
        Task DeleteDeviceAsync(Guid id);
    }
}
