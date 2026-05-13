using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DemoKROS.DTO.Company;

namespace DemoKROS.Entities;

[Table("Companies")]
public class CompanyEntity : OrganizationNodeEntity
{
    public List<EmployeeEntity> Employees { get; set; } = new();

    public List<DivisionEntity> Divisions { get; set; } = new();
    
    public CompanyResponse ToResponse()
    {
        return new CompanyResponse(
            Id,
            Name,
            Code,
            LeaderId
        );
    }
}