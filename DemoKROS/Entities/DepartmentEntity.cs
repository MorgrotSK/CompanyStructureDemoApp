using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DemoKROS.DTO.Departments;


namespace DemoKROS.Entities;

[Table("Departments")]
public class DepartmentEntity : OrganizationNodeEntity
{
    [ForeignKey(nameof(ProjectEntity))]
    public int ProjectId { get; set; }
    public virtual ProjectEntity ProjectEntity { get; set; } = null!;
    
    [NotMapped]
    public override OrganizationNodeEntity? ParentNode => ProjectEntity;
    
    public DepartmentResponse ToResponse()
    {
        return new DepartmentResponse(
            Id,
            Name,
            Code,
            ProjectId,
            LeaderId
        );
    }
}