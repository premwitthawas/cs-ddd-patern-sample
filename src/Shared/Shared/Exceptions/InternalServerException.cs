namespace Shared.Exceptions;

public class InternalServerException : Exception
{
    public string? Detials { get; set; }
    public InternalServerException(string msg) : base(message: msg)
    {

    }

    public InternalServerException(string message, string detials)
        : base(message)
    {
        Detials = detials;
    }
}