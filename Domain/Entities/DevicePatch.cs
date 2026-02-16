using OneGlobal.Domain.Enums;

namespace OneGlobal.Domain.Entities
{
    public record DevicePatch
    {
        public string? Name { get; set; }
        public string? Brand { get; set; }
        public State? State { get; set; }

        public DevicePatch(string? name, string? brand, State? state)
        {
            Name = name;
            Brand = brand;
            State = state;
        }
    }
}
