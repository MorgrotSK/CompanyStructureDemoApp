using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DemoKROS.DTO.Projects;

namespace DemoKROS.Entities;

[Table("Projects")]
public class ProjectEntity : OrganizationNodeEntity
{
    [ForeignKey(nameof(DivisionEntity))]
    public int DivisionId { get; set; }

    public DivisionEntity DivisionEntity { get; set; } = null!;

    public List<DepartmentEntity> Departments { get; set; } = new();
    
    public ProjectResponse ToResponse()
    {
        return new ProjectResponse(
            Id,
            Name,
            Code,
            DivisionId,
            LeaderId
        );
    }
}