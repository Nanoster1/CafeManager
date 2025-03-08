namespace CafeManager.Core.Exceptions;

public class EntityConflictException : Exception
{
    public EntityConflictException()
    {
    }

    public EntityConflictException(string? message) : base(message)
    {
    }
}