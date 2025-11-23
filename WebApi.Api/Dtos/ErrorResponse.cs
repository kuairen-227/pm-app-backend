namespace WebApi.Api.Dtos;

public class ErrorResponse
{
    public ErrorDetail Error { get; set; } = default!;
}

public class ErrorDetail
{
    public string Code { get; set; } = default!;
    public string Message { get; set; } = default!;
    public string TraceId { get; set; } = default!;
}
