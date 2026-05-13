using DemoKROS.DTO.Common;
using DemoKROS.DTO.Divisions;
using DemoKROS.DTO.Employees;
using DemoKROS.DTO.Projects;
using DemoKROS.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemoKROS.Controllers;

[ApiController]
[Route("api/divisions")]
public class DivisionsController(DivisionsService divisionsService, ProjectsService projectsService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<DivisionResponse>>> GetAll()
    {
        var divisions = await divisionsService.GetAllAsync();
        return Ok(divisions);
    }

    [HttpGet("{divisionId:int}")]
    [ProducesResponseType(typeof(DivisionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DivisionResponse>> GetById(int divisionId)
    {
        var division = await divisionsService.GetByIdAsync(divisionId);
        return Ok(division);
    }
    
    [HttpGet("{divisionId:int}/projects")]
    [ProducesResponseType(typeof(List<ProjectResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async  Task<ActionResult<List<ProjectResponse>>> GetDivisionProjects(int divisionId)
    {
        var projects = await divisionsService.GetDivisionProjectsAsync(divisionId);
        return Ok(projects);
    }
    
    [HttpPost("{divisionId:int}/projects")]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProjectResponse>>  CreateNewDivisionProject(int divisionId, CreateProjectRequest request)
    {
        var project = await projectsService.CreateAsync(request, divisionId);
        return CreatedAtAction(
            nameof(ProjectsController.GetById),
            "Projects",
            new { projectId = project.Id },
            project
        );
    }
    
    [HttpPatch("{divisionId:int}")]
    [ProducesResponseType(typeof(DivisionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DivisionResponse>> Update(int divisionId, UpdateOrganizationNodeRequest request)
    {
        var division = await divisionsService.UpdateAsync(divisionId, request);
        return Ok(division);
    }
    
    [HttpDelete("{divisionId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int divisionId)
    {
        await divisionsService.DeleteAsync(divisionId);
        return NoContent();
    }
    
    [HttpGet("{divisionId:int}/leader")]
    [ProducesResponseType(typeof(EmployeeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmployeeResponse>> GetLeader(int divisionId)
    {
        var leader = await divisionsService.GetLeaderAsync(divisionId);
        return Ok(leader);
    }

    [HttpPut("{divisionId:int}/leader/{leaderId:int}")]
    [ProducesResponseType(typeof(DivisionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DivisionResponse>> SetLeader(int divisionId, int leaderId)
    {
        var division = await divisionsService.SetLeaderAsync(divisionId, leaderId);
        return Ok(division);
    }

    [HttpDelete("{divisionId:int}/leader")]
    [ProducesResponseType(typeof(DivisionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DivisionResponse>> RemoveLeader(int divisionId)
    {
        var division = await divisionsService.RemoveLeaderAsync(divisionId);
        return Ok(division);
    }
}