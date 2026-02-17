using MM.Domain.Enums;

namespace MM.Domain.Entities
{
    public record DevicePatch
    {
        public string? Name { get; set; }
        public string? Brand { get; set; }
        public State? State { get; set; }

        public DevicePatch(string? name = null, string? brand = null, State? state = null)
        {
            Name = name;
            Brand = brand;
            State = state;
        }
    }
}
