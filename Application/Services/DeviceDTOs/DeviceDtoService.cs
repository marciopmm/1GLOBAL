using AutoMapper;
using Global.Application.Abstractions.Services.DeviceDTOs;
using Global.Application.DTOs;
using Global.Domain.Entities;
using Global.Domain.Enums;
using Global.Domain.Ports;

namespace Global.Application.Services.DeviceDTOs
{
    public class DeviceDtoService : IDeviceDtoService
    {
        private readonly IDeviceService _deviceService;
        private readonly IMapper _mapper;

        public DeviceDtoService(IDeviceService deviceService, IMapper mapper)
        {
            _deviceService = deviceService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DeviceDTO>> GetAllDevicesAsync()
        {
            var devices = await _deviceService.GetAllDevicesAsync();
            return _mapper.Map<IEnumerable<DeviceDTO>>(devices);
        }

        public async Task<IEnumerable<DeviceDTO>> GetDevicesByStateAsync(string state)
        {
            if (!Enum.TryParse<State>(state, true, out var parsedState))
            {
                throw new ArgumentException("Invalid state value.");
            }

            var devices = await _deviceService.GetDevicesByStateAsync(parsedState);
            return _mapper.Map<IEnumerable<DeviceDTO>>(devices);
        }

        public async Task<IEnumerable<DeviceDTO>> GetDevicesByBrandAsync(string brand)
        {
            var devices = await _deviceService.GetDevicesByBrandAsync(brand);
            return _mapper.Map<IEnumerable<DeviceDTO>>(devices);
        }
        
        public async Task<DeviceDTO> GetDeviceByIdAsync(int id)
        {
            var device = await _deviceService.GetDeviceByIdAsync(id);
            return _mapper.Map<DeviceDTO>(device);
        }

        public async Task<DeviceDTO> AddDeviceAsync(DeviceDTO deviceDto)
        {
            var device = _mapper.Map<Device>(deviceDto);
            await _deviceService.AddDeviceAsync(device);
        }

        public async Task UpdateDeviceAsync(DeviceDTO deviceDto)
        {
            var device = _mapper.Map<Device>(deviceDto);
            await _deviceService.UpdateDeviceAsync(device);
        }

        public async Task DeleteDeviceAsync(int id)
        {
            await _deviceService.DeleteDeviceAsync(id);
        }
    }
}