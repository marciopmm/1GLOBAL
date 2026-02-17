namespace MM.Domain.Exceptions;

public class InvalidStateForUpdateException : Exception
{
    public InvalidStateForUpdateException(Guid id)
        : base($"Invalid State to update 'Name' or 'Brand' for device with ID {id}.")
    {
    }
}