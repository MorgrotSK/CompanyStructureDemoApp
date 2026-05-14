using DemoKROS.Data;
using DemoKROS.DTO.Common;
using DemoKROS.DTO.Company;
using DemoKROS.DTO.Divisions;
using DemoKROS.DTO.Employees;
using DemoKROS.Entities;
using Microsoft.EntityFrameworkCore;

namespace DemoKROS.Services;

public class CompaniesService(AppDbContext dbContext, OrganizationNodeService organizationNodeService)
{
    public async Task<List<CompanyResponse>> GetAllAsync()
    {
        return await dbContext.Companies.Select(c => c.ToResponse()).ToListAsync();
    }

    public async Task<ServiceResult<CompanyResponse>> GetByIdAsync(int id)
    {
        CompanyEntity? company = await dbContext.Companies.FirstOrDefaultAsync(c => c.Id == id);

        if (company == null) return ServiceResult<CompanyResponse>.NotFound("Company not found.");

        return ServiceResult<CompanyResponse>.Ok(company.ToResponse());
    }
    
    public async Task<ServiceResult<List<EmployeeResponse>>> GetEmployeesAsync(int companyId)
    {
        if (!await DbValidationHelpers.EntityExistsAsync(dbContext.Companies, companyId))
            return ServiceResult<List<EmployeeResponse>>.NotFound("Company not found.");

        var employees = await dbContext.Employees.Where(e => e.CompanyId == companyId).Select(e => e.ToResponse()).ToListAsync();

        return ServiceResult<List<EmployeeResponse>>.Ok(employees);
    }
    
    public async Task<ServiceResult<List<DivisionResponse>>> GetDivisionsAsync(int companyId)
    {
        if (!await DbValidationHelpers.EntityExistsAsync(dbContext.Companies, companyId))
            return ServiceResult<List<DivisionResponse>>.NotFound("Company not found.");

        var divisions = await dbContext.Divisions.Where(d => d.CompanyId == companyId).Select(d => d.ToResponse()).ToListAsync();

        return ServiceResult<List<DivisionResponse>>.Ok(divisions);
    }

    public async Task<ServiceResult<CompanyResponse>> UpdateAsync(int companyId, UpdateOrganizationNodeRequest request)
    {
        var result = await organizationNodeService.UpdateAsync(dbContext.Companies, dbContext.Companies, companyId, request);

        if (!result.Success)
            return new ServiceResult<CompanyResponse> { Success = false, Error = result.Error, StatusCode = result.StatusCode };

        return ServiceResult<CompanyResponse>.Ok(result.Data!.ToResponse());
    }

    public async Task<ServiceResult<CompanyResponse>> CreateAsync(CreateCompanyRequest request)
    {
        bool codeExists = await dbContext.Companies.AnyAsync(c => c.Code == request.Code);

        if (codeExists) return ServiceResult<CompanyResponse>.BadRequest("Company code already exists.");

        CompanyEntity companyEntity = new()
        {
            Name = request.Name,
            Code = request.Code,
        };

        dbContext.Companies.Add(companyEntity);
        await dbContext.SaveChangesAsync();

        return ServiceResult<CompanyResponse>.Ok(companyEntity.ToResponse());
    }
    
    public async Task<ServiceResult> DeleteAsync(int companyId)
    {
        CompanyEntity? company = await dbContext.Companies.FirstOrDefaultAsync(c => c.Id == companyId);

        if (company == null) return ServiceResult.NotFound("Company not found.");

        dbContext.Companies.Remove(company);
        await dbContext.SaveChangesAsync();

        return ServiceResult.Ok();
    }
    
    public async Task<ServiceResult<EmployeeResponse>> GetLeaderAsync(int companyId)
    {
        return await organizationNodeService.GetLeaderAsync(dbContext.Companies, companyId);
    }
    
    public async Task<ServiceResult<CompanyResponse>> SetLeaderAsync(int companyId, int leaderId)
    {
        var result = await organizationNodeService.SetLeaderAsync(dbContext.Companies, companyId, leaderId);

        if (!result.Success)
            return new ServiceResult<CompanyResponse> { Success = false, Error = result.Error, StatusCode = result.StatusCode };

        return ServiceResult<CompanyResponse>.Ok(result.Data!.ToResponse());
    }

    public async Task<ServiceResult<CompanyResponse>> RemoveLeaderAsync(int companyId)
    {
        var result = await organizationNodeService.RemoveLeaderAsync(dbContext.Companies, companyId);

        if (!result.Success)
            return new ServiceResult<CompanyResponse> { Success = false, Error = result.Error, StatusCode = result.StatusCode };

        return ServiceResult<CompanyResponse>.Ok(result.Data!.ToResponse());
    }
}