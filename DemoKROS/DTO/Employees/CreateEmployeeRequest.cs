using System.ComponentModel.DataAnnotations;

namespace DemoKROS.DTO.Employees;

public class CreateEmployeeRequest
{

    [MaxLength(30)] public string? Title { get; set; } = "";

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = "";

    [Required]
    [MaxLength(50)]
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