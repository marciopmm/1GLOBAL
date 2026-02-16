using OneGlobal.Domain.Enums;

namespace OneGlobal.Domain.Entities
{
    public record Device
    {
        public Guid Id { get; private set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public State State { get; set; }
        public DateTime CreationTime { get; private set; }

        public Device(string name, string brand, State state, DateTime creationTime)
        {
            Id = Guid.NewGuid();
            Name = name;
            Brand = brand;
            State = state;
            CreationTime = creationTime;
        }
    }
}
