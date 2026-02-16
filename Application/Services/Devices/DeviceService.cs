using AutoMapper;
using OneGlobal.Application.DTOs;
using OneGlobal.Application.Validations;
using OneGlobal.Domain.Entities;
using OneGlobal.Domain.Enums;
using OneGlobal.Domain.Exceptions;
using OneGlobal.Domain.Ports;

namespace OneGlobal.Application.Services.Devices
{
    internal class DeviceService : IDeviceService
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
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Device>> GetDevicesByBrandAsync(string brand)
        {
            throw new NotImplementedException();
        }

        public async Task<Device> GetDeviceByIdAsync(Guid id)
        {
            return await _deviceRepository.GetByIdAsync(id);
        }

        public async Task<Device> AddDeviceAsync(Device device)
        {
            return await _deviceRepository.AddAsync(device);
        }

        public async Task<Device> UpdateDeviceAsync(Guid id, DevicePatch devicePatch)
        {
            await ValidateDeviceForUpdate(id, devicePatch);
            return await _deviceRepository.UpdateAsync(id, devicePatch);
        }

        public async Task<Device> UpdateDevicePartialAsync(Guid id, DevicePatch devicePatch)
        {
            await ValidateDeviceForUpdate(id, devicePatch);
            return await _deviceRepository.UpdatePartialAsync(id, devicePatch);
        }

        public async Task DeleteDeviceAsync(Guid id)
        {
            await ValidateDeviceForDelete(id);
            await _deviceRepository.DeleteAsync(id);
        }

        private async Task ValidateDeviceForUpdate(Guid id, DevicePatch patch)
        {
            var current = await _deviceRepository.GetByIdAsync(id);
            if (!DeviceValidations.IsValidForUpdate(current, patch))
            {
                throw new InvalidStateForUpdateException(current.Id);
            }
        }

        private async Task ValidateDeviceForDelete(Guid id)
        {
            var device = await _deviceRepository.GetByIdAsync(id);
            if (!DeviceValidations.IsValidForDelete(device))
            {
                throw new InvalidStateForDeleteException(device.Id);
            }
        }
    }
}
