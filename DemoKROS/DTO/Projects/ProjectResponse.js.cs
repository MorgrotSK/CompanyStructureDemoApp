namespace DemoKROS.DTO.Projects;

public record ProjectResponse(
    int Id,
    string Name,
    string Code,
    int DivisionId,
    int? LeaderId
);