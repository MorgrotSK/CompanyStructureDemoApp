using DemoKROS.Constants;
using DemoKROS.DTO.Common;
using DemoKROS.DTO.Divisions;
using DemoKROS.DTO.Employees;
using DemoKROS.DTO.Projects;
using DemoKROS.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemoKROS.Controllers;

[ApiController]
[Route(ApiRoutes.Divisions)]
public class DivisionsController(DivisionsService divisionsService, ProjectsService projectsService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(List<DivisionResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<DivisionResponse>>> GetAll()
    {
        var divisions = await divisionsService.GetAllAsync();
        return Ok(divisions);
    }

    [HttpGet(ApiRoutes.DivisionsRoutes.ById, Name = ApiRoutes.RouteNames.GetDivisionById)]
    [ProducesResponseType(typeof(DivisionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DivisionResponse>> GetById(int divisionId)
    {
        var result = await divisionsService.GetByIdAsync(divisionId);

        if (!result.Success) return StatusCode(result.StatusCode, new { error = result.Error });

        return Ok(result.Data);
    }

    [HttpGet(ApiRoutes.DivisionsRoutes.Projects)]
    [ProducesResponseType(typeof(List<ProjectResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<ProjectResponse>>> GetDivisionProjects(int divisionId)
    {
        var result = await divisionsService.GetDivisionProjectsAsync(divisionId);

        if (!result.Success) return StatusCode(result.StatusCode, new { error = result.Error });

        return Ok(result.Data);
    }

    [HttpPost(ApiRoutes.DivisionsRoutes.Projects)]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProjectResponse>> CreateNewDivisionProject(int divisionId, CreateProjectRequest request)
    {
        var result = await projectsService.CreateAsync(request, divisionId);

        if (!result.Success) return StatusCode(result.StatusCode, new { error = result.Error });

        return CreatedAtRoute(ApiRoutes.RouteNames.GetProjectById, new { projectId = result.Data!.Id }, result.Data);
    }

    [HttpPatch(ApiRoutes.DivisionsRoutes.ById)]
    [ProducesResponseType(typeof(DivisionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DivisionResponse>> Update(int divisionId, UpdateOrganizationNodeRequest request)
    {
        var result = await divisionsService.UpdateAsync(divisionId, request);

        if (!result.Success) return StatusCode(result.StatusCode, new { error = result.Error });

        return Ok(result.Data);
    }

    [HttpDelete(ApiRoutes.DivisionsRoutes.ById)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int divisionId)
    {
        var result = await divisionsService.DeleteAsync(divisionId);

        if (!result.Success) return StatusCode(result.StatusCode, new { error = result.Error });

        return NoContent();
    }

    [HttpGet(ApiRoutes.DivisionsRoutes.Leader)]
    [ProducesResponseType(typeof(EmployeeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmployeeResponse>> GetLeader(int divisionId)
    {
        var result = await divisionsService.GetLeaderAsync(divisionId);

        if (!result.Success) return StatusCode(result.StatusCode, new { error = result.Error });

        return Ok(result.Data);
    }

    [HttpPut(ApiRoutes.DivisionsRoutes.LeaderById)]
    [ProducesResponseType(typeof(DivisionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DivisionResponse>> SetLeader(int divisionId, int leaderId)
    {
        var result = await divisionsService.SetLeaderAsync(divisionId, leaderId);

        if (!result.Success) return StatusCode(result.StatusCode, new { error = result.Error });

        return Ok(result.Data);
    }

    [HttpDelete(ApiRoutes.DivisionsRoutes.Leader)]
    [ProducesResponseType(typeof(DivisionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DivisionResponse>> RemoveLeader(int divisionId)
    {
        var result = await divisionsService.RemoveLeaderAsync(divisionId);

        if (!result.Success) return StatusCode(result.StatusCode, new { error = result.Error });

        return Ok(result.Data);
    }
}