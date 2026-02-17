namespace MM.Domain.Exceptions;

public class InvalidStateForDeleteException : Exception
{
    public InvalidStateForDeleteException(Guid id)
        : base($"Invalid state to delete device with ID {id}.")
    {
    }
}