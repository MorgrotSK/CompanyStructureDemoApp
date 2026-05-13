using System.ComponentModel.DataAnnotations;

namespace DemoKROS.DTO.Common;

public class UpdateOrganizationNodeRequest
{
    [MaxLength(100)]
    public string Name { get; set; } = "";
    [MaxLength(20)]
    public string Code { get; set; } = "";
}