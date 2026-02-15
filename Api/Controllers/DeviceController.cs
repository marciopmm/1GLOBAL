using Microsoft.AspNetCore.Mvc;
using Global.Application.DTOs;
using Global.Application.Abstractions.Services;

namespace Global.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceDtoService _deviceDtoService;

        public DeviceController(IDeviceDtoService deviceDtoService)
        {
            _deviceDtoService = deviceDtoService;
        }

        [HttpGet(Name = "Devices")]
        public async Task<IEnumerable<DeviceDTO>> Get()
        {
            return await _deviceDtoService.GetAllDevicesAsync();
        }

        [HttpPost(Name = "Devices")]
        public async Task Post()
        {
            var newDevice = new DeviceDTO
            {
                Name = "New Device",
                Brand = "BrandX",
                State = "Active"
            };
            
            return await _deviceDtoService.AddDeviceAsync(newDevice);
        }
    }
}
