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
        [ProducesResponseType(typeof(IEnumerable<DeviceDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IEnumerable<DeviceDTO>> Get()
        {
            return await _deviceDtoService.GetAllDevicesAsync();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DeviceDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
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
            catch (DeviceNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(DeviceDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<DeviceDTO>>> GetByQuery(
            [FromQuery] string? name,
            [FromQuery] string? brand,
            [FromQuery] string? state)
        {
            try
            {
                var device = await _deviceDtoService.GetDevicesByQueryAsync(name, brand, state);
                return Ok(device);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPost()]
        [ProducesResponseType(typeof(DeviceDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DeviceDTO>> Post([FromBody] AddDeviceDtoRequest addDeviceDto)
        {
            try
            {
                return Created(string.Empty, await _deviceDtoService.AddDeviceAsync(addDeviceDto));
            }
            catch (Exception ex) when (ex is ArgumentOutOfRangeException ||
                                       ex is ArgumentNullException ||
                                       ex is ArgumentException ||
                                       ex is InvalidStateException)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(DeviceDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DeviceDTO>> Put(Guid id, [FromBody] UpdateDeviceDtoRequest updateDeviceDto)
        {
            try
            {
                return Ok(await _deviceDtoService.UpdateDeviceAsync(id, updateDeviceDto));
            }
            catch (Exception ex) when (ex is ArgumentNullException ||
                                       ex is ArgumentException ||
                                       ex is InvalidStateForUpdateException ||
                                       ex is InvalidStateException)
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
        [ProducesResponseType(typeof(DeviceDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DeviceDTO>> Patch(Guid id, [FromBody] UpdateDeviceDtoRequest updateDeviceDto)
        {
            try
            {
                return Ok(await _deviceDtoService.UpdateDevicePartialAsync(id, updateDeviceDto));
            }
            catch (Exception ex) when (ex is ArgumentNullException ||
                                       ex is ArgumentException ||
                                       ex is InvalidStateForUpdateException ||
                                       ex is InvalidStateException)
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
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _deviceDtoService.DeleteDeviceAsync(id);
                return NoContent();
            }
            catch (Exception ex) when (ex is ArgumentNullException ||
                                       ex is ArgumentException ||
                                       ex is InvalidStateForDeleteException)
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
    }
}
