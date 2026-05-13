using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DemoKROS.DTO.Divisions;

namespace DemoKROS.Entities;

[Table("Divisions")]
public class DivisionEntity : OrganizationNodeEntity
{
    public int CompanyId { get; set; }

    public CompanyEntity CompanyEntity { get; set; } = null!;

    public List<ProjectEntity> Projects { get; set; } = new();
    
    public DivisionResponse ToResponse()
    {
        return new DivisionResponse(
            Id,
            Name,
            Code,
            CompanyId,
            LeaderId
        );
    }
}