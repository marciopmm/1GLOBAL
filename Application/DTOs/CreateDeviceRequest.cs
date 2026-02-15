using Global.Domain.Enums;

namespace Global.Application.DTOs
{
    public class CreateDeviceRequest
    {
        public string Name { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public string State { get; set; }
    }
}