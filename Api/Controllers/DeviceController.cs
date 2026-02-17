using Microsoft.AspNetCore.Mvc;
using MM.Application.DTOs;
using MM.Application.Abstractions.Services;
using MM.Domain.Entities;
using MM.Domain.Exceptions;

namespace MM.Api.Controllers
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
        public async Task<ActionResult<DeviceDTO>> Get(Guid id)
        {
            try
            {
                var device = await _deviceDtoService.GetDeviceByIdAsync(id);
                if (device == null)
                {
                    return NotFound();
                }
                return Ok(device);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPost()]
        public async Task<ActionResult<DeviceDTO>> Post([FromBody] AddDeviceDtoRequest addDeviceDto)
        {
            try
            {
                return await _deviceDtoService.AddDeviceAsync(addDeviceDto);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest($"The field {ex.ParamName} is required.");
            }
            catch (InvalidStateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<DeviceDTO>> Put(Guid id, [FromBody] UpdateDeviceDtoRequest updateDeviceDto)
        {
            try
            {
                return await _deviceDtoService.UpdateDeviceAsync(id, updateDeviceDto);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest($"The field {ex.ParamName} is required.");
            }
            catch (InvalidStateForUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidStateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DeviceNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<DeviceDTO>> Patch(Guid id, [FromBody] UpdateDeviceDtoRequest updateDeviceDto)
        {
            try
            {
                return await _deviceDtoService.UpdateDevicePartialAsync(id, updateDeviceDto);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest($"The field {ex.ParamName} is required.");
            }
            catch (InvalidStateForUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidStateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DeviceNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _deviceDtoService.DeleteDeviceAsync(id);
                return NoContent();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest($"The field {ex.ParamName} is required.");
            }
            catch (DeviceNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
