using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoKROS.Entities;

public abstract class OrganizationNodeEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = "";

    [Required]
    [MaxLength(20)]
    public string Code { get; set; } = "";

    [ForeignKey(nameof(EmployeeEntity))]
    public int? LeaderId { get; set; }
    public virtual EmployeeEntity? Leader { get; set; }
    
    [NotMapped]
    public virtual OrganizationNodeEntity? ParentNode => null;

    public CompanyEntity? GetCompany()
    {
        OrganizationNodeEntity? current = this;

        while (current is not null)
        {
            if (current is CompanyEntity company)
                return company;

            current = current.ParentNode;
        }

        return null;
    }
}