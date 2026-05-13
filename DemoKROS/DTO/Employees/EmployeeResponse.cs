namespace DemoKROS.DTO.Employees;


public record EmployeeResponse(
    int Id,
    string? Title,
    string FirstName,
    string LastName,
    string Phone,
    string Email,
    int CompanyId
);