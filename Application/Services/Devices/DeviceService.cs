using AutoMapper;
using MM.Application.DTOs;
using MM.Application.Validations;
using MM.Domain.Entities;
using MM.Domain.Enums;
using MM.Domain.Exceptions;
using MM.Domain.Ports;

namespace MM.Application.Services.Devices
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository _deviceRepository;

        public DeviceService(IDeviceRepository deviceRepository)
        {
            _deviceRepository = deviceRepository;
        }

        public async Task<IEnumerable<Device>> GetAllDevicesAsync()
        {
            return await _deviceRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Device>> GetDevicesByStateAsync(State state)
        {
            var devices = await _deviceRepository.GetAllAsync();
            return devices.Where(d => d.State == state);
        }

        public async Task<IEnumerable<Device>> GetDevicesByBrandAsync(string brand)
        {
            if (string.IsNullOrWhiteSpace(brand))
            {
                throw new ArgumentNullException(nameof(brand));
            }
            var devices = await _deviceRepository.GetAllAsync();
            return devices.Where(d => d.Brand.ToLower() == brand.ToLower());
        }

        public async Task<Device> GetDeviceByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }
            return await _deviceRepository.GetByIdAsync(id);
        }

        public async Task<Device> AddDeviceAsync(Device device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }
            await ValidateDeviceForAdd(device);
            return await _deviceRepository.AddAsync(device);
        }

        public async Task<Device> UpdateDeviceAsync(Guid id, DevicePatch devicePatch)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }
            if (devicePatch == null)
            {
                throw new ArgumentNullException(nameof(devicePatch));
            }
            await ValidateDeviceForUpdate(id, devicePatch);
            return await _deviceRepository.UpdateAsync(id, devicePatch);
        }

        public async Task<Device> UpdateDevicePartialAsync(Guid id, DevicePatch devicePatch)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }
            if (devicePatch == null)
            {
                throw new ArgumentNullException(nameof(devicePatch));
            }
            await ValidateDeviceForUpdate(id, devicePatch);
            return await _deviceRepository.UpdatePartialAsync(id, devicePatch);
        }

        public async Task DeleteDeviceAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }
            await ValidateDeviceForDelete(id);
            await _deviceRepository.DeleteAsync(id);
        }

        private async Task ValidateDeviceForAdd(Device device)
        {
            DeviceValidations.IsValidForAdd(device);
        }

        private async Task ValidateDeviceForUpdate(Guid id, DevicePatch patch)
        {
            var current = await _deviceRepository.GetByIdAsync(id);
            DeviceValidations.IsValidForUpdate(current, patch);
        }

        private async Task ValidateDeviceForDelete(Guid id)
        {
            var device = await _deviceRepository.GetByIdAsync(id);
            DeviceValidations.IsValidForDelete(device);
        }
    }
}
