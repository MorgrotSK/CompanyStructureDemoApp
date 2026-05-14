using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DemoKROS.DTO.Company;

namespace DemoKROS.Entities;

[Table("Companies")]
public class CompanyEntity : OrganizationNodeEntity
{
    public virtual List<EmployeeEntity> Employees { get; set; } = new();

    public virtual List<DivisionEntity> Divisions { get; set; } = new();
    
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