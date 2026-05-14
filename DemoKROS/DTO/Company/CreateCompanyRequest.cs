using System.ComponentModel.DataAnnotations;
using DemoKROS.Constants;

namespace DemoKROS.DTO.Company;

public class CreateCompanyRequest
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    [RegularExpression(ValidationPatterns.OrganizationName)]
    public string Name { get; set; } = "";

    [Required]
    [StringLength(20, MinimumLength = 2)]
    [RegularExpression(ValidationPatterns.OrganizationCode)]
    [MaxLength(20)]
    public string Code { get; set; } = "";
}