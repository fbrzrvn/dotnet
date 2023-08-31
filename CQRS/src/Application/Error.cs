namespace Application;

public class Error
{
    public ErrorCode Code { get; set; }

    public string Message { get; set; }
}

public enum ErrorCode
{
    BadRequest,
    NotFound,
    Conflict,
    InternalServerError
}