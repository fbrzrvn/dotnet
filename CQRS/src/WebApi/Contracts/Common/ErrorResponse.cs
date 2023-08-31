namespace WebApi.Contracts.Common;

public class ErrorResponse
{
    public int StatusCode { get; set; }

    public string Message { get; set; }

    public List<string> Errors { get; set; } = new();

    public DateTime TimeStamp { get; set; }
}