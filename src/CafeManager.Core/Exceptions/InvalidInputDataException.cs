namespace CafeManager.Core.Exceptions;

public class InvalidInputDataException : Exception
{
    public InvalidInputDataException()
    {
    }

    public InvalidInputDataException(string? message) : base(message)
    {
    }
}