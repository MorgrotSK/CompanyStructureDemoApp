using DemoKROS.Constants;
using DemoKROS.DTO.Common;
using DemoKROS.DTO.Departments;
using DemoKROS.DTO.Employees;
using DemoKROS.Extensions;
using DemoKROS.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemoKROS.Controllers;

[ApiController]
[Route(ApiRoutes.Departments)]
public class DepartmentsController(DepartmentsService departmentsService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(List<DepartmentResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<DepartmentResponse>>> GetAll()
    {
        var departments = await departmentsService.GetAllAsync();
        return Ok(departments);
    }

    [HttpGet(ApiRoutes.DepartmentsRoutes.ById, Name = ApiRoutes.RouteNames.GetDepartmentById)]
    [ProducesResponseType(typeof(DepartmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DepartmentResponse>> GetById(int departmentId)
    {
        var result = await departmentsService.GetByIdAsync(departmentId);

        if (!result.Success) return this.ToActionResult(result);

        return Ok(result.Data);
    }

    [HttpPatch(ApiRoutes.DepartmentsRoutes.ById)]
    [ProducesResponseType(typeof(DepartmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DepartmentResponse>> Update(int departmentId, UpdateOrganizationNodeRequest request)
    {
        var result = await departmentsService.UpdateAsync(departmentId, request);

        if (!result.Success) return this.ToActionResult(result);

        return Ok(result.Data);
    }

    [HttpDelete(ApiRoutes.DepartmentsRoutes.ById)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int departmentId)
    {
        var result = await departmentsService.DeleteAsync(departmentId);

        if (!result.Success) return this.ToEmptyActionResult(result);

        return NoContent();
    }

    [HttpGet(ApiRoutes.DepartmentsRoutes.Leader)]
    [ProducesResponseType(typeof(EmployeeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmployeeResponse>> GetLeader(int departmentId)
    {
        var result = await departmentsService.GetLeaderAsync(departmentId);

        if (!result.Success) return this.ToActionResult(result);

        return Ok(result.Data);
    }

    [HttpPut(ApiRoutes.DepartmentsRoutes.LeaderById)]
    [ProducesResponseType(typeof(DepartmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DepartmentResponse>> SetLeader(int departmentId, int leaderId)
    {
        var result = await departmentsService.SetLeaderAsync(departmentId, leaderId);

        if (!result.Success) return this.ToActionResult(result);

        return Ok(result.Data);
    }

    [HttpDelete(ApiRoutes.DepartmentsRoutes.Leader)]
    [ProducesResponseType(typeof(DepartmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DepartmentResponse>> RemoveLeader(int departmentId)
    {
        var result = await departmentsService.RemoveLeaderAsync(departmentId);

        if (!result.Success) return this.ToActionResult(result);

        return Ok(result.Data);
    }
}