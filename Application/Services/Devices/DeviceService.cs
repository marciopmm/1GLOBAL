using AutoMapper;
using Global.Application.DTOs;
using Global.Domain.Entities;
using Global.Domain.Enums;
using Global.Domain.Ports;

namespace Global.Application.Services.Devices
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
        public async Task<Device> GetDeviceByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<Device> AddDeviceAsync(Device device)
        {
            return await _deviceRepository.AddAsync(device);
        }
        public async Task UpdateDeviceAsync(Device device)
        {
            throw new NotImplementedException();
        }
        public async Task DeleteDeviceAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
