using System.ComponentModel.DataAnnotations;

namespace DemoKROS.DTO.Employees;

public class UpdateEmployeeRequest
{
    [MaxLength(30)]
    public string? Title { get; set; }

    [MaxLength(50)]
    public string? FirstName { get; set; }

    [MaxLength(50)]
    public string? LastName { get; set; }

    [Phone]
    [MaxLength(30)]
    public string? Phone { get; set; }

    [EmailAddress]
    [MaxLength(100)]
    public string? Email { get; set; }
}