namespace DemoKROS.DTO.Divisions;

public record DivisionResponse(
    int Id,
    string Name,
    string Code,
    int CompanyId,
    int? LeaderId
);