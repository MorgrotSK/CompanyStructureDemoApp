using DemoKROS.Constants;
using DemoKROS.DTO.Employees;
using DemoKROS.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemoKROS.Controllers;

[ApiController]
[Route(ApiRoutes.Employees)]
public class EmployeesController(EmployeesService employeesService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(List<EmployeeResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<EmployeeResponse>>> GetAll()
    {
        var employees = await employeesService.GetAllAsync();
        return Ok(employees);
    }

    [HttpGet(ApiRoutes.EmployeesRoutes.ById, Name = ApiRoutes.RouteNames.GetEmployeeById)]
    [ProducesResponseType(typeof(EmployeeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmployeeResponse>> GetById(int employeeId)
    {
        var result = await employeesService.GetByIdAsync(employeeId);

        if (!result.Success)
            return StatusCode(result.StatusCode, new { error = result.Error });

        return Ok(result.Data);
    }

    [HttpPatch(ApiRoutes.EmployeesRoutes.ById)]
    [ProducesResponseType(typeof(EmployeeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EmployeeResponse>> Update(int employeeId, UpdateEmployeeRequest request)
    {
        var result = await employeesService.UpdateAsync(employeeId, request);

        if (!result.Success)
            return StatusCode(result.StatusCode, new { error = result.Error });

        return Ok(result.Data);
    }

    [HttpDelete(ApiRoutes.EmployeesRoutes.ById)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int employeeId)
    {
        var result = await employeesService.DeleteAsync(employeeId);

        if (!result.Success)
            return StatusCode(result.StatusCode, new { error = result.Error });

        return NoContent();
    }
}