using DemoKROS.DTO.Employees;
using DemoKROS.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemoKROS.Controllers;

[ApiController]
[Route("api/employees")]
public class EmployeesController(EmployeesService employeesService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(List<EmployeeResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<EmployeeResponse>>> GetAll()
    {
        var employees = await employeesService.GetAllAsync();
        return Ok(employees);
    }

    [HttpGet("{employeeId:int}")]
    [ProducesResponseType(typeof(EmployeeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmployeeResponse>> GetById(int employeeId)
    {
        var employee = await employeesService.GetByIdAsync(employeeId);
        return Ok(employee);
    }
    
    [HttpPatch("{employeeId:int}")]
    [ProducesResponseType(typeof(EmployeeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EmployeeResponse>> Update(int employeeId, UpdateEmployeeRequest request)
    {
        var employee = await employeesService.UpdateAsync(employeeId, request);
        return Ok(employee);
    }

    [HttpDelete("{employeeId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int employeeId)
    {
        await employeesService.DeleteAsync(employeeId);
        return NoContent();
    }
}