namespace MM.Domain.Exceptions;

public class InvalidStateException : Exception
{
    public InvalidStateException()
        : base($"Invalid State data for device.")
    {
    }

    public InvalidStateException(Guid id)
        : base($"Invalid State for device with ID {id}.")
    {
    }
}