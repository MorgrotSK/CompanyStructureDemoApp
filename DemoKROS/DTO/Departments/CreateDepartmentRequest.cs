using System.ComponentModel.DataAnnotations;
using DemoKROS.Constants;

namespace DemoKROS.DTO.Departments;

public class CreateDepartmentRequest
{
    [Required]
    [MaxLength(100)]
    [RegularExpression(ValidationPatterns.OrganizationName)]
    public string Name { get; set; } = "";

    [Required]
    [MaxLength(20)]
    [RegularExpression(ValidationPatterns.OrganizationCode)]
    public string Code { get; set; } = "";

    public int? LeaderId { get; set; }
}