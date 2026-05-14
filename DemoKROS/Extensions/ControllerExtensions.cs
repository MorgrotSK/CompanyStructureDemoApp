using DemoKROS.DTO.Common;
using Microsoft.AspNetCore.Mvc;

namespace DemoKROS.Extensions;

public static class ControllerExtensions
{
    public static ActionResult<T> ToActionResult<T>(this ControllerBase controller, ServiceResult<T> result)
    {
        if (!result.Success)
            return controller.StatusCode(result.StatusCode, new { error = result.Error });

        return controller.Ok(result.Data);
    }

    public static IActionResult ToEmptyActionResult(this ControllerBase controller, ServiceResult result)
    {
        if (!result.Success)
            return controller.StatusCode(result.StatusCode, new { error = result.Error });

        return controller.Ok();
    }
}