using DemoKROS.DTO.Common;
using DemoKROS.DTO.Departments;
using DemoKROS.DTO.Employees;
using DemoKROS.DTO.Projects;
using DemoKROS.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemoKROS.Controllers;

[ApiController]
[Route("api/projects")]
public class ProjectsController(ProjectsService projectsService, DepartmentsService departmentsService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(List<ProjectResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ProjectResponse>>> GetAll()
    {
        var projects = await projectsService.GetAllAsync();
        return Ok(projects);
    }

    [HttpGet("{projectId:int}")]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProjectResponse>> GetById(int projectId)
    {
        var project = await projectsService.GetByIdAsync(projectId);
        return Ok(project);
    }
    
    [HttpGet("{projectId:int}/departments")]
    [ProducesResponseType(typeof(List<DepartmentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<DepartmentResponse>>> GetProjectDepartments(int projectId)
    {
        var departments = await projectsService.GetProjectDepartmentsAsync(projectId);
        return Ok(departments);
    }
    
    [HttpPost("{projectId:int}/departments")]
    [ProducesResponseType(typeof(DepartmentResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DepartmentResponse>> Create(int projectId, CreateDepartmentRequest request)
    {
        var department = await departmentsService.CreateAsync(request, projectId);

        return Created($"/api/departments/{department.Id}", department
        );
    }
    
    [HttpPatch("{projectId:int}")]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProjectResponse>> Update(int projectId, UpdateOrganizationNodeRequest request)
    {
        var project = await projectsService.UpdateAsync(projectId, request);
        return Ok(project);
    }

    [HttpDelete("{projectId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int projectId)
    {
        await projectsService.DeleteAsync(projectId);

        return NoContent();
    }
    
    [HttpGet("{projectId:int}/leader")]
    [ProducesResponseType(typeof(EmployeeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmployeeResponse>> GetLeader(int projectId)
    {
        var leader = await projectsService.GetLeaderAsync(projectId);
        return Ok(leader);
    }

    [HttpPut("{projectId:int}/leader/{leaderId:int}")]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProjectResponse>> SetLeader(int projectId, int leaderId)
    {
        var project = await projectsService.SetLeaderAsync(projectId, leaderId);
        return Ok(project);
    }

    [HttpDelete("{projectId:int}/leader")]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProjectResponse>> RemoveLeader(int projectId)
    {
        var project = await projectsService.RemoveLeaderAsync(projectId);
        return Ok(project);
    }
}