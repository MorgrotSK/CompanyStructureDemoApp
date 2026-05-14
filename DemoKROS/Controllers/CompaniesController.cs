using DemoKROS.Constants;
using DemoKROS.DTO.Common;
using DemoKROS.DTO.Company;
using DemoKROS.DTO.Divisions;
using DemoKROS.DTO.Employees;
using DemoKROS.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemoKROS.Controllers;

[ApiController]
[Route(ApiRoutes.Companies)]
public class CompaniesController(CompaniesService companiesService, DivisionsService divisionsService, EmployeesService employeesService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(List<CompanyResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CompanyResponse>>> GetAll()
    {
        var companies = await companiesService.GetAllAsync();
        return Ok(companies);
    }
    
    [HttpGet(ApiRoutes.CompaniesRoutes.ById, Name = ApiRoutes.RouteNames.GetCompanyById)]
    [ProducesResponseType(typeof(CompanyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CompanyResponse>> GetById(int companyId)
    {
        var result = await companiesService.GetByIdAsync(companyId);

        if (!result.Success) return StatusCode(result.StatusCode, new { error = result.Error });

        return Ok(result.Data);
    }

    [HttpGet(ApiRoutes.CompaniesRoutes.Employees)]
    [ProducesResponseType(typeof(List<EmployeeResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<EmployeeResponse>>> GetEmployees(int companyId)
    {
        var result = await companiesService.GetEmployeesAsync(companyId);

        if (!result.Success) return StatusCode(result.StatusCode, new { error = result.Error });

        return Ok(result.Data);
    }

    [HttpPost(ApiRoutes.CompaniesRoutes.Employees)]
    [ProducesResponseType(typeof(EmployeeResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EmployeeResponse>> CreateNewCompanyEmployee(int companyId, CreateEmployeeRequest request)
    {
        var result = await employeesService.CreateAsync(request, companyId);

        if (!result.Success) return StatusCode(result.StatusCode, new { error = result.Error });

        return CreatedAtRoute(ApiRoutes.RouteNames.GetEmployeeById, new { employeeId = result.Data!.Id }, result.Data);
    }

    [HttpGet(ApiRoutes.CompaniesRoutes.Divisions)]
    [ProducesResponseType(typeof(List<DivisionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<DivisionResponse>>> GetDivisions(int companyId)
    {
        var result = await companiesService.GetDivisionsAsync(companyId);

        if (!result.Success) return StatusCode(result.StatusCode, new { error = result.Error });

        return Ok(result.Data);
    }

    [HttpPost(ApiRoutes.CompaniesRoutes.Divisions)]
    [ProducesResponseType(typeof(DivisionResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DivisionResponse>> CreateNewCompanyDivision(int companyId, CreateDivisionRequest request)
    {
        var result = await divisionsService.CreateAsync(request, companyId);

        if (!result.Success) return StatusCode(result.StatusCode, new { error = result.Error });

        return CreatedAtRoute(ApiRoutes.RouteNames.GetDivisionById, new { divisionId = result.Data!.Id }, result.Data);
    }

    [HttpPatch(ApiRoutes.CompaniesRoutes.ById)]
    [ProducesResponseType(typeof(CompanyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CompanyResponse>> Update(int companyId, UpdateOrganizationNodeRequest request)
    {
        var result = await companiesService.UpdateAsync(companyId, request);

        if (!result.Success) return StatusCode(result.StatusCode, new { error = result.Error });

        return Ok(result.Data);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CompanyResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CompanyResponse>> Create(CreateCompanyRequest request)
    {
        var result = await companiesService.CreateAsync(request);

        if (!result.Success) return StatusCode(result.StatusCode, new { error = result.Error });

        return CreatedAtAction(nameof(GetById), new { companyId = result.Data!.Id }, result.Data);
    }

    [HttpDelete(ApiRoutes.CompaniesRoutes.ById)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int companyId)
    {
        var result = await companiesService.DeleteAsync(companyId);

        if (!result.Success) return StatusCode(result.StatusCode, new { error = result.Error });

        return NoContent();
    }

    [HttpGet(ApiRoutes.CompaniesRoutes.Leader)]
    [ProducesResponseType(typeof(EmployeeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmployeeResponse>> GetLeader(int companyId)
    {
        var result = await companiesService.GetLeaderAsync(companyId);

        if (!result.Success) return StatusCode(result.StatusCode, new { error = result.Error });

        return Ok(result.Data);
    }

    [HttpPut(ApiRoutes.CompaniesRoutes.LeaderById)]
    [ProducesResponseType(typeof(CompanyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CompanyResponse>> SetLeader(int companyId, int leaderId)
    {
        var result = await companiesService.SetLeaderAsync(companyId, leaderId);

        if (!result.Success) return StatusCode(result.StatusCode, new { error = result.Error });

        return Ok(result.Data);
    }

    [HttpDelete(ApiRoutes.CompaniesRoutes.Leader)]
    [ProducesResponseType(typeof(CompanyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CompanyResponse>> RemoveLeader(int companyId)
    {
        var result = await companiesService.RemoveLeaderAsync(companyId);

        if (!result.Success) return StatusCode(result.StatusCode, new { error = result.Error });

        return Ok(result.Data);
    }
}