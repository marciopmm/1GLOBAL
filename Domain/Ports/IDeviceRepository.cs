using System;
using System.Collections.Generic;
using System.Text;
using MM.Domain.Entities;

namespace MM.Domain.Ports
{
    public interface IDeviceRepository
    {
        Task<Device> GetByIdAsync(Guid id);
        Task<IEnumerable<Device>> GetAllAsync();
        Task<Device> AddAsync(Device device);
        Task<Device> UpdateAsync(Guid id, DevicePatch devicePatch);
        Task<Device> UpdatePartialAsync(Guid id, DevicePatch devicePatch);
        Task DeleteAsync(Guid id);
    }
}
