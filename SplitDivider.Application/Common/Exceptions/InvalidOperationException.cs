namespace SplitDivider.Application.Common.Exceptions;

public class InvalidOperationException : Exception
{
    public InvalidOperationException() : base() { }
    
    public InvalidOperationException(string message) : base(message) { }
}