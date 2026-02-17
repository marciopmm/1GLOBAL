namespace MM.Application.DTOs
{
    public class DeviceDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public string State { get; set; } = null!;
        public DateTime CreationTime { get; set; }
    }
}