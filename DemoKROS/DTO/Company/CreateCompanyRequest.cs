using System.ComponentModel.DataAnnotations;

namespace DemoKROS.DTO.Company;

public class CreateCompanyRequest
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = "";

    [Required]
    [MaxLength(20)]
    public string Code { get; set; } = "";

    public int? LeaderId { get; set; }
}