using System.ComponentModel.DataAnnotations;
using DemoKROS.Constants;

namespace DemoKROS.DTO.Employees;

public class UpdateEmployeeRequest
{
    private string? _title;
    private string? _firstName;
    private string? _lastName;
    private string? _phone;
    private string? _email;

    [MaxLength(30)]
    public string? Title
    {
        get => _title;
        set => _title = string.IsNullOrWhiteSpace(value) ? null : value;
    }

    [StringLength(50, MinimumLength = 2)]
    [RegularExpression(ValidationPatterns.PersonName)]
    public string? FirstName
    {
        get => _firstName;
        set => _firstName = string.IsNullOrWhiteSpace(value) ? null : value;
    }

    [StringLength(50, MinimumLength = 2)]
    [RegularExpression(ValidationPatterns.PersonName)]
    public string? LastName
    {
        get => _lastName;
        set => _lastName = string.IsNullOrWhiteSpace(value) ? null : value;
    }

    [Phone]
    [MaxLength(30)]
    public string? Phone
    {
        get => _phone;
        set => _phone = string.IsNullOrWhiteSpace(value) ? null : value;
    }

    [EmailAddress]
    [MaxLength(100)]
    public string? Email
    {
        get => _email;
        set => _email = string.IsNullOrWhiteSpace(value) ? null : value;
    }
}