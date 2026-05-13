namespace DemoKROS.DTO.Departments;

public record DepartmentResponse(
    int Id,
    string Name,
    string Code,
    int ProjectId,
    int? LeaderId
);