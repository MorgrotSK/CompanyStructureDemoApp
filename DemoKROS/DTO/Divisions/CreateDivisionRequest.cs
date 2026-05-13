using System.ComponentModel.DataAnnotations;

namespace DemoKROS.DTO.Divisions;

public record CreateDivisionRequest(
    [Required]
    [MaxLength(100)]
    string Name,

    [Required]
    [MaxLength(20)]
    string Code,

    int? LeaderId
);