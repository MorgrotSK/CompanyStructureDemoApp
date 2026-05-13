using DemoKROS.DTO.Common;
using DemoKROS.DTO.Departments;
using DemoKROS.DTO.Employees;
using DemoKROS.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemoKROS.Controllers;

[ApiController]
[Route("api/departments")]
public class DepartmentsController(DepartmentsService departmentsService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(List<DepartmentResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<DepartmentResponse>>> GetAll()
    {
        var departments = await departmentsService.GetAllAsync();
        return Ok(departments);
    }

    [HttpGet("{departmentId:int}")]
    [ProducesResponseType(typeof(DepartmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DepartmentResponse>> GetById(int departmentId)
    {
        var department = await departmentsService.GetByIdAsync(departmentId);
        return Ok(department);
    }
    
    [HttpPatch("{departmentId:int}")]
    [ProducesResponseType(typeof(DepartmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DepartmentResponse>> Update(int departmentId, UpdateOrganizationNodeRequest request)
    {
        var department = await departmentsService.UpdateAsync(departmentId, request);
        return Ok(department);
    }
    
    [HttpDelete("{departmentId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int departmentId)
    {
        await departmentsService.DeleteAsync(departmentId);
        return NoContent();
    }
    
    [HttpGet("{departmentId:int}/leader")]
    [ProducesResponseType(typeof(EmployeeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmployeeResponse>> GetLeader(int departmentId)
    {
        var leader = await departmentsService.GetLeaderAsync(departmentId);
        return Ok(leader);
    }

    [HttpPut("{departmentId:int}/leader/{leaderId:int}")]
    [ProducesResponseType(typeof(DepartmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DepartmentResponse>> SetLeader(int departmentId, int leaderId)
    {
        var department = await departmentsService.SetLeaderAsync(departmentId, leaderId);
        return Ok(department);
    }

    [HttpDelete("{departmentId:int}/leader")]
    [ProducesResponseType(typeof(DepartmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DepartmentResponse>> RemoveLeader(int departmentId)
    {
        var department = await departmentsService.RemoveLeaderAsync(departmentId);
        return Ok(department);
    }
}