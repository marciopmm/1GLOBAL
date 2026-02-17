namespace MM.Domain.Exceptions;

public class DeviceNotFoundException : Exception
{
    public DeviceNotFoundException(Guid id)
        : base($"Device with ID {id} not found.")
    {
    }
}