namespace DemoKROS.DTO.Common;

public class ServiceResult
{
    public bool Success { get; init; }
    public string? Error { get; init; }
    public int StatusCode { get; init; }

    public static ServiceResult Ok() => new() { Success = true, StatusCode = StatusCodes.Status204NoContent };
    public static ServiceResult NotFound(string error) => new() { Success = false, Error = error, StatusCode = StatusCodes.Status404NotFound };
    public static ServiceResult BadRequest(string error) => new() { Success = false, Error = error, StatusCode = StatusCodes.Status400BadRequest };
}