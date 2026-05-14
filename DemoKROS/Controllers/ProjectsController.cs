using DemoKROS.Constants;
using DemoKROS.DTO.Common;
using DemoKROS.DTO.Departments;
using DemoKROS.DTO.Employees;
using DemoKROS.DTO.Projects;
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
        var project = await projectsService.GetByIdAsync(projectId);
        return Ok(project);
    }

    [HttpGet(ApiRoutes.ProjectsRoutes.Departments)]
    [ProducesResponseType(typeof(List<DepartmentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<DepartmentResponse>>> GetProjectDepartments(int projectId)
    {
        var departments = await projectsService.GetProjectDepartmentsAsync(projectId);
        return Ok(departments);
    }

    [HttpPost(ApiRoutes.ProjectsRoutes.Departments)]
    [ProducesResponseType(typeof(DepartmentResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DepartmentResponse>> Create(int projectId, CreateDepartmentRequest request)
    {
        var department = await departmentsService.CreateAsync(request, projectId);
        return CreatedAtRoute(ApiRoutes.RouteNames.GetDepartmentById, new { departmentId = department.Id }, department);    }

    [HttpPatch(ApiRoutes.ProjectsRoutes.ById)]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProjectResponse>> Update(int projectId, UpdateOrganizationNodeRequest request)
    {
        var project = await projectsService.UpdateAsync(projectId, request);
        return Ok(project);
    }

    [HttpDelete(ApiRoutes.ProjectsRoutes.ById)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int projectId)
    {
        await projectsService.DeleteAsync(projectId);
        return NoContent();
    }

    [HttpGet(ApiRoutes.ProjectsRoutes.Leader)]
    [ProducesResponseType(typeof(EmployeeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmployeeResponse>> GetLeader(int projectId)
    {
        var leader = await projectsService.GetLeaderAsync(projectId);
        return Ok(leader);
    }

    [HttpPut(ApiRoutes.ProjectsRoutes.LeaderById)]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProjectResponse>> SetLeader(int projectId, int leaderId)
    {
        var project = await projectsService.SetLeaderAsync(projectId, leaderId);
        return Ok(project);
    }

    [HttpDelete(ApiRoutes.ProjectsRoutes.Leader)]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProjectResponse>> RemoveLeader(int projectId)
    {
        var project = await projectsService.RemoveLeaderAsync(projectId);
        return Ok(project);
    }
}