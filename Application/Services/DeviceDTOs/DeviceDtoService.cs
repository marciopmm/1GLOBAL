using AutoMapper;
using MM.Application.Abstractions.Services;
using MM.Application.DTOs;
using MM.Domain.Entities;
using MM.Domain.Enums;
using MM.Domain.Ports;

namespace MM.Application.Services.DeviceDTOs
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

        public async Task<DeviceDTO> GetDeviceByIdAsync(Guid id)
        {
            var device = await _deviceService.GetDeviceByIdAsync(id);
            return _mapper.Map<DeviceDTO>(device);
        }

        public async Task<DeviceDTO> AddDeviceAsync(AddDeviceDtoRequest addDeviceDto)
        {
            var device = _mapper.Map<Device>(addDeviceDto);
            var addedDevice = await _deviceService.AddDeviceAsync(device);
            return _mapper.Map<DeviceDTO>(addedDevice);
        }

        public async Task<DeviceDTO> UpdateDeviceAsync(Guid id, UpdateDeviceDtoRequest deviceDto)
        {
            var devicePatch = _mapper.Map<DevicePatch>(deviceDto);
            var updatedDevice = await _deviceService.UpdateDeviceAsync(id, devicePatch);
            return _mapper.Map<DeviceDTO>(updatedDevice);
        }

        public async Task<DeviceDTO> UpdateDevicePartialAsync(Guid id, UpdateDeviceDtoRequest deviceDto)
        {
            var devicePatch = _mapper.Map<DevicePatch>(deviceDto);
            var updatedDevice = await _deviceService.UpdateDevicePartialAsync(id, devicePatch);
            return _mapper.Map<DeviceDTO>(updatedDevice);
        }

        public async Task DeleteDeviceAsync(Guid id)
        {
            await _deviceService.DeleteDeviceAsync(id);
        }
    }
}