using DemoKROS.Constants;
using DemoKROS.DTO.Common;
using DemoKROS.DTO.Departments;
using DemoKROS.DTO.Employees;
using DemoKROS.DTO.Projects;
using DemoKROS.Extensions;
using DemoKROS.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemoKROS.Controllers;

[ApiController]
[Route(ApiRoutes.Projects)]
public class ProjectsController(ProjectsService projectsService, DepartmentsService departmentsService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(List<ProjectResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ProjectResponse>>> GetAll()
    {
        var projects = await projectsService.GetAllAsync();
        return Ok(projects);
    }

    [HttpGet(ApiRoutes.ProjectsRoutes.ById, Name = ApiRoutes.RouteNames.GetProjectById)]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProjectResponse>> GetById(int projectId)
    {
        var result = await projectsService.GetByIdAsync(projectId);

        if (!result.Success) return this.ToActionResult(result);

        return Ok(result.Data);
    }

    [HttpGet(ApiRoutes.ProjectsRoutes.Departments)]
    [ProducesResponseType(typeof(List<DepartmentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<DepartmentResponse>>> GetProjectDepartments(int projectId)
    {
        var result = await projectsService.GetProjectDepartmentsAsync(projectId);

        if (!result.Success) return this.ToActionResult(result);

        return Ok(result.Data);
    }

    [HttpPost(ApiRoutes.ProjectsRoutes.Departments)]
    [ProducesResponseType(typeof(DepartmentResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DepartmentResponse>> Create(int projectId, CreateDepartmentRequest request)
    {
        var result = await departmentsService.CreateAsync(request, projectId);

        if (!result.Success) return this.ToActionResult(result);

        return CreatedAtRoute(ApiRoutes.RouteNames.GetDepartmentById, new { departmentId = result.Data!.Id }, result.Data);
    }

    [HttpPatch(ApiRoutes.ProjectsRoutes.ById)]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProjectResponse>> Update(int projectId, UpdateOrganizationNodeRequest request)
    {
        var result = await projectsService.UpdateAsync(projectId, request);

        if (!result.Success) return this.ToActionResult(result);

        return Ok(result.Data);
    }

    [HttpDelete(ApiRoutes.ProjectsRoutes.ById)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int projectId)
    {
        var result = await projectsService.DeleteAsync(projectId);

        if (!result.Success) return this.ToEmptyActionResult(result);

        return NoContent();
    }

    [HttpGet(ApiRoutes.ProjectsRoutes.Leader)]
    [ProducesResponseType(typeof(EmployeeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmployeeResponse>> GetLeader(int projectId)
    {
        var result = await projectsService.GetLeaderAsync(projectId);

        if (!result.Success) return this.ToActionResult(result);

        return Ok(result.Data);
    }

    [HttpPut(ApiRoutes.ProjectsRoutes.LeaderById)]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProjectResponse>> SetLeader(int projectId, int leaderId)
    {
        var result = await projectsService.SetLeaderAsync(projectId, leaderId);

        if (!result.Success) return this.ToActionResult(result);

        return Ok(result.Data);
    }

    [HttpDelete(ApiRoutes.ProjectsRoutes.Leader)]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProjectResponse>> RemoveLeader(int projectId)
    {
        var result = await projectsService.RemoveLeaderAsync(projectId);

        if (!result.Success) return this.ToActionResult(result);

        return Ok(result.Data);
    }
}