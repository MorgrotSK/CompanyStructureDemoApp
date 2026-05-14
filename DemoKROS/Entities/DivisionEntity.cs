using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DemoKROS.DTO.Divisions;

namespace DemoKROS.Entities;

[Table("Divisions")]
public class DivisionEntity : OrganizationNodeEntity
{
    [ForeignKey(nameof(CompanyEntity))]
    public int CompanyId { get; set; }

    public virtual CompanyEntity CompanyEntity { get; set; } = null!;

    public virtual List<ProjectEntity> Projects { get; set; } = new();
    
    [NotMapped]
    public override OrganizationNodeEntity? ParentNode => CompanyEntity;
    
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