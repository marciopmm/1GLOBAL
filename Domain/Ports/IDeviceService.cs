using MM.Domain.Entities;
using MM.Domain.Enums;

namespace MM.Domain.Ports
{
    public interface IDeviceService
    {
        Task<IEnumerable<Device>> GetAllDevicesAsync();
        Task<Device> GetDeviceByIdAsync(Guid id);
        Task<Device> AddDeviceAsync(Device device);
        Task<Device> UpdateDeviceAsync(Guid id, DevicePatch devicePatch);
        Task<Device> UpdateDevicePartialAsync(Guid id, DevicePatch devicePatch);
        Task DeleteDeviceAsync(Guid id);
    }
}
