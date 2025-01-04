namespace Shared.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    {

    }

    public BadRequestException(string message, string detials)
        : base(message)
    {
        Detials = detials;
    }

    public string? Detials { get; set; }
}