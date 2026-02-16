namespace OneGlobal.Domain.Exceptions;

public class InvalidStateForUpdateException : Exception
{
    public InvalidStateForUpdateException(Guid id)
        : base($"Invalid state to update 'Name' or 'Brand' data for device with ID {id}.")
    {
    }
}