namespace DemoKROS.DTO.Common;

public class ServiceResult<T>
{
    public bool Success { get; init; }
    public T? Data { get; init; }
    public string? Error { get; init; }
    public int StatusCode { get; init; }

    public static ServiceResult<T> Ok(T data) => new()
    {
        Success = true,
        Data = data,
        StatusCode = StatusCodes.Status200OK
    };

    public static ServiceResult<T> NotFound(string error) => new()
    {
        Success = false,
        Error = error,
        StatusCode = StatusCodes.Status404NotFound
    };

    public static ServiceResult<T> BadRequest(string error) => new()
    {
        Success = false,
        Error = error,
        StatusCode = StatusCodes.Status400BadRequest
    };
    
    public static ServiceResult<T> Fail(ServiceResult result) => new()
    {
        Success = false,
        Error = result.Error,
        StatusCode = result.StatusCode
    };
    
    public static ServiceResult<T> Fail<TSource>(ServiceResult<TSource> result) => new()
    {
        Success = false,
        Error = result.Error,
        StatusCode = result.StatusCode
    };
}