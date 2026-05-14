using System.ComponentModel.DataAnnotations;
using DemoKROS.Constants;

namespace DemoKROS.DTO.Divisions;

public record CreateDivisionRequest(
    [Required]
    [MaxLength(100)]
    [RegularExpression(ValidationPatterns.OrganizationName)]
    string Name,

    [Required]
    [MaxLength(20)]
    [RegularExpression(ValidationPatterns.OrganizationCode)]
    string Code,

    int? LeaderId
);