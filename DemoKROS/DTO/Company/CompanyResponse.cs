namespace DemoKROS.DTO.Company;

public record CompanyResponse(
    int Id,
    string Name,
    string Code,
    int? LeaderId
);