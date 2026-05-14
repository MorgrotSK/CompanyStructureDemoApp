using System.ComponentModel.DataAnnotations;
using DemoKROS.Constants;

namespace DemoKROS.DTO.Employees;

public class CreateEmployeeRequest
{

    [MaxLength(30)] 
    public string? Title { get; set; } = "";

    [Required]
    [StringLength(50, MinimumLength = 2)]
    [RegularExpression(ValidationPatterns.PersonName)]
    public string FirstName { get; set; } = "";

    [Required]
    [StringLength(50, MinimumLength = 2)]
    [RegularExpression(ValidationPatterns.PersonName)]
    public string LastName { get; set; } = "";

    [Required]
    [Phone]
    [MaxLength(30)]
    public string Phone { get; set; } = "";

    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; } = "";
}