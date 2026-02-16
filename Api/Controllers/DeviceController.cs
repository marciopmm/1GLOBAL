using Microsoft.AspNetCore.Mvc;
using OneGlobal.Application.DTOs;
using OneGlobal.Application.Abstractions.Services;
using OneGlobal.Domain.Entities;

namespace OneGlobal.Api.Controllers
{
    [ApiController]
    [Route("devices")]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceDtoService _deviceDtoService;

        public DeviceController(IDeviceDtoService deviceDtoService)
        {
            _deviceDtoService = deviceDtoService;
        }

        [HttpGet()]
        public async Task<IEnumerable<DeviceDTO>> Get()
        {
            return await _deviceDtoService.GetAllDevicesAsync();
        }

        [HttpGet("{id}")]
        public async Task<DeviceDTO> Get(Guid id)
        {
            return await _deviceDtoService.GetDeviceByIdAsync(id);
        }

        [HttpPost()]
        public async Task<DeviceDTO> Post([FromBody] AddDeviceDtoRequest addDeviceDto)
        {
            return await _deviceDtoService.AddDeviceAsync(addDeviceDto);
        }

        [HttpPut("{id}")]
        public async Task<DeviceDTO> Put(Guid id, [FromBody] UpdateDeviceDtoRequest updateDeviceDto)
        {
            return await _deviceDtoService.UpdateDeviceAsync(id, updateDeviceDto);
        }

        [HttpPatch("{id}")]
        public async Task<DeviceDTO> Patch(Guid id, [FromBody] UpdateDeviceDtoRequest updateDeviceDto)
        {
            return await _deviceDtoService.UpdateDevicePartialAsync(id, updateDeviceDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _deviceDtoService.DeleteDeviceAsync(id);
            return NoContent();
        }
    }
}
