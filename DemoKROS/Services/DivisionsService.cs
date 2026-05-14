using DemoKROS.Data;
using DemoKROS.DTO.Common;
using DemoKROS.DTO.Divisions;
using DemoKROS.DTO.Employees;
using DemoKROS.DTO.Projects;
using DemoKROS.Entities;
using Microsoft.EntityFrameworkCore;

namespace DemoKROS.Services;

public class DivisionsService(AppDbContext dbContext, OrganizationNodeService organizationNodeService)
{
    public async Task<List<DivisionResponse>> GetAllAsync()
    {
        return await dbContext.Divisions.Select(d => d.ToResponse()).ToListAsync();
    }

    public async Task<ServiceResult<DivisionResponse>> GetByIdAsync(int id)
    {
        DivisionEntity? division = await dbContext.Divisions.FirstOrDefaultAsync(d => d.Id == id);

        if (division == null) return ServiceResult<DivisionResponse>.NotFound("Division not found.");

        return ServiceResult<DivisionResponse>.Ok(division.ToResponse());
    }
    
    public async Task<ServiceResult<List<ProjectResponse>>> GetDivisionProjectsAsync(int divisionId)
    {
        if (!await DbValidationHelpers.EntityExistsAsync(dbContext.Divisions, divisionId))
            return ServiceResult<List<ProjectResponse>>.NotFound("Division not found.");

        var projects = await dbContext.Projects.Where(p => p.DivisionId == divisionId).Select(p => p.ToResponse()).ToListAsync();

        return ServiceResult<List<ProjectResponse>>.Ok(projects);
    }

    public async Task<ServiceResult<DivisionResponse>> CreateAsync(CreateDivisionRequest request, int companyId)
    {
        if (!await DbValidationHelpers.EntityExistsAsync(dbContext.Companies, companyId))
            return ServiceResult<DivisionResponse>.NotFound("Company not found.");

        bool codeExists = await dbContext.Divisions.AnyAsync(d => d.CompanyId == companyId && d.Code == request.Code);

        if (codeExists) return ServiceResult<DivisionResponse>.BadRequest("Division code already exists for the company.");

        DivisionEntity divisionEntity = new()
        {
            Name = request.Name,
            Code = request.Code,
            CompanyId = companyId
        };

        dbContext.Divisions.Add(divisionEntity);
        await dbContext.SaveChangesAsync();

        if (request.LeaderId is not null)
        {
            var leaderResult = await organizationNodeService.SetLeaderAsync(dbContext.Divisions, divisionEntity.Id, request.LeaderId.Value);

            if (!leaderResult.Success) return ServiceResult<DivisionResponse>.Fail(leaderResult);

            divisionEntity = leaderResult.Data!;
        }

        return ServiceResult<DivisionResponse>.Ok(divisionEntity.ToResponse());
    }

    public async Task<ServiceResult> DeleteAsync(int divisionId)
    {
        DivisionEntity? division = await dbContext.Divisions.FirstOrDefaultAsync(d => d.Id == divisionId);

        if (division == null) return ServiceResult.NotFound("Division not found.");

        dbContext.Divisions.Remove(division);
        await dbContext.SaveChangesAsync();

        return ServiceResult.NoContent();
    }
    
    public async Task<ServiceResult<DivisionResponse>> UpdateAsync(int divisionId, UpdateOrganizationNodeRequest request)
    {
        DivisionEntity? division = await dbContext.Divisions.FirstOrDefaultAsync(d => d.Id == divisionId);

        if (division == null) return ServiceResult<DivisionResponse>.NotFound("Division not found.");

        var result = await organizationNodeService.UpdateAsync(dbContext.Divisions, dbContext.Divisions.Where(d => d.CompanyId == division.CompanyId), divisionId, request);

        if (!result.Success) return ServiceResult<DivisionResponse>.Fail(result);

        return ServiceResult<DivisionResponse>.Ok(result.Data!.ToResponse());
    }
    
    public async Task<ServiceResult<EmployeeResponse>> GetLeaderAsync(int divisionId)
    {
        return await organizationNodeService.GetLeaderAsync(dbContext.Divisions, divisionId);
    }

    public async Task<ServiceResult<DivisionResponse>> SetLeaderAsync(int divisionId, int leaderId)
    {
        var result = await organizationNodeService.SetLeaderAsync(dbContext.Divisions, divisionId, leaderId);

        if (!result.Success) return ServiceResult<DivisionResponse>.Fail(result);

        return ServiceResult<DivisionResponse>.Ok(result.Data!.ToResponse());
    }

    public async Task<ServiceResult<DivisionResponse>> RemoveLeaderAsync(int divisionId)
    {
        var result = await organizationNodeService.RemoveLeaderAsync(dbContext.Divisions, divisionId);

        if (!result.Success) return ServiceResult<DivisionResponse>.Fail(result);

        return ServiceResult<DivisionResponse>.Ok(result.Data!.ToResponse());
    }
}