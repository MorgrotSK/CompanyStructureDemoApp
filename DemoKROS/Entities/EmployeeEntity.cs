using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DemoKROS.DTO.Employees;

namespace DemoKROS.Entities;

[Table("Employees")]
public class EmployeeEntity
{
    [Key]
    public int Id { get; set; }
    
    [MaxLength(30)] 
    public string Title { get; set; } = "";
    
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = "";
    
    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = "";
    
    [Required]
    [MaxLength(30)]
    public string Phone { get; set; } = "";
    
    [Required]
    [MaxLength(100)]
    public string Email { get; set; } = "";
    
    [ForeignKey(nameof(CompanyEntity))]
    public int CompanyId { get; set; }
    public virtual CompanyEntity CompanyEntity { get; set; } = null!;
    
    public EmployeeResponse ToResponse()
    {
        return new EmployeeResponse(
            Id,
            Title,
            FirstName,
            LastName,
            Phone,
            Email,
            CompanyId
        );
    }
}