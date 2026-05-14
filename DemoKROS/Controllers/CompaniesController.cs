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
        var company = await companiesService.GetByIdAsync(companyId);
        return Ok(company);
    }

    [HttpGet(ApiRoutes.CompaniesRoutes.Employees)]
    [ProducesResponseType(typeof(List<EmployeeResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<EmployeeResponse>>> GetEmployees(int companyId)
    {
        var employees = await companiesService.GetEmployeesAsync(companyId);
        return Ok(employees);
    }

    [HttpPost(ApiRoutes.CompaniesRoutes.Employees)]
    [ProducesResponseType(typeof(EmployeeResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EmployeeResponse>> CreateNewCompanyEmployee(int companyId, CreateEmployeeRequest request)
    {
        var employee = await employeesService.CreateAsync(request, companyId);
        return CreatedAtRoute(ApiRoutes.RouteNames.GetEmployeeById, new { employeeId = employee.Id }, employee);    }

    [HttpGet(ApiRoutes.CompaniesRoutes.Divisions)]
    [ProducesResponseType(typeof(List<DivisionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<DivisionResponse>>> GetDivisions(int companyId)
    {
        var divisions = await companiesService.GetDivisionsAsync(companyId);
        return Ok(divisions);
    }

    [HttpPost(ApiRoutes.CompaniesRoutes.Divisions)]
    [ProducesResponseType(typeof(DivisionResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DivisionResponse>> CreateNewCompanyDivision(int companyId, CreateDivisionRequest request)
    {
        var division = await divisionsService.CreateAsync(request, companyId);
        return CreatedAtRoute(ApiRoutes.RouteNames.GetDivisionById, new { divisionId = division.Id }, division);    }

    [HttpPatch(ApiRoutes.CompaniesRoutes.ById)]
    public async Task<ActionResult<CompanyResponse>> Update(int companyId, UpdateOrganizationNodeRequest request)
    {
        var company = await companiesService.UpdateAsync(companyId, request);
        return Ok(company);
    }

    [HttpPost]
    public async Task<ActionResult<CompanyResponse>> Create(CreateCompanyRequest request)
    {
        var company = await companiesService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { companyId = company.Id }, company);
    }

    [HttpDelete(ApiRoutes.CompaniesRoutes.ById)]
    public async Task<IActionResult> Delete(int companyId)
    {
        await companiesService.DeleteAsync(companyId);
        return NoContent();
    }

    [HttpGet(ApiRoutes.CompaniesRoutes.Leader)]
    public async Task<ActionResult<EmployeeResponse>> GetLeader(int companyId)
    {
        var leader = await companiesService.GetLeaderAsync(companyId);
        return Ok(leader);
    }

    [HttpPut(ApiRoutes.CompaniesRoutes.LeaderById)]
    public async Task<ActionResult<CompanyResponse>> SetLeader(int companyId, int leaderId)
    {
        var company = await companiesService.SetLeaderAsync(companyId, leaderId);
        return Ok(company);
    }

    [HttpDelete(ApiRoutes.CompaniesRoutes.Leader)]
    public async Task<ActionResult<CompanyResponse>> RemoveLeader(int companyId)
    {
        var company = await companiesService.RemoveLeaderAsync(companyId);
        return Ok(company);
    }
}