using System.ComponentModel.DataAnnotations;
using DemoKROS.Constants;

namespace DemoKROS.DTO.Common;

public class UpdateOrganizationNodeRequest
{
    private string? _name;
    private string? _code;

    [StringLength(100, MinimumLength = 2)]
    [RegularExpression(ValidationPatterns.OrganizationName)]
    public string? Name
    {
        get => _name;
        set => _name = string.IsNullOrWhiteSpace(value) ? null : value;
    }

    [StringLength(20, MinimumLength = 2)]
    [RegularExpression(ValidationPatterns.OrganizationCode)]
    public string? Code
    {
        get => _code;
        set => _code = string.IsNullOrWhiteSpace(value) ? null : value;
    }
}