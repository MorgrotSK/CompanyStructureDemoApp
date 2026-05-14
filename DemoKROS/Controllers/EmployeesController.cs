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
    public async Task<ActionResult<List<EmployeeResponse>>> GetAll()
    {
        var employees = await employeesService.GetAllAsync();
        return Ok(employees);
    }

    
    [HttpGet(ApiRoutes.EmployeesRoutes.ById, Name = ApiRoutes.RouteNames.GetEmployeeById)]
    public async Task<ActionResult<EmployeeResponse>> GetById(int employeeId)
    {
        var employee = await employeesService.GetByIdAsync(employeeId);
        return Ok(employee);
    }

    [HttpPatch(ApiRoutes.EmployeesRoutes.ById)]
    public async Task<ActionResult<EmployeeResponse>> Update(int employeeId, UpdateEmployeeRequest request)
    {
        var employee = await employeesService.UpdateAsync(employeeId, request);
        return Ok(employee);
    }

    [HttpDelete(ApiRoutes.EmployeesRoutes.ById)]
    public async Task<IActionResult> Delete(int employeeId)
    {
        await employeesService.DeleteAsync(employeeId);
        return NoContent();
    }
}