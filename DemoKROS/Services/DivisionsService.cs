using DemoKROS.Data;
using DemoKROS.DTO.Common;
using DemoKROS.DTO.Company;
using DemoKROS.DTO.Divisions;
using DemoKROS.DTO.Employees;
using DemoKROS.DTO.Projects;
using DemoKROS.Entities;
using DemoKROS.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DemoKROS.Services;

public class DivisionsService(AppDbContext dbContext, OrganizationNodeService organizationNodeService)
{
    public async Task<List<DivisionResponse>> GetAllAsync()
    {
        return await dbContext.Divisions.Select(d => new DivisionResponse(d.Id, d.Name, d.Code, d.CompanyId, d.LeaderId)).ToListAsync();
    }

    public async Task<DivisionResponse?> GetByIdAsync(int id)
    {
        DivisionEntity? division = await dbContext.Divisions.FirstOrDefaultAsync(d => d.Id == id);
        if (division == null) throw new NotFoundException("Division not found.");
        return division.ToResponse();
    }
    
    public async Task<List<ProjectResponse>> GetDivisionProjectsAsync(int divisionId)
    {
        return await dbContext.Projects
            .Where(p => p.DivisionId == divisionId)
            .Select(p => new ProjectResponse(p.Id, p.Name, p.Code, p.DivisionId, p.LeaderId))
            .ToListAsync();
    }

    public async Task<DivisionResponse> CreateAsync(CreateDivisionRequest request, int companyId)
    {
        await DbValidationHelpers.EnsureEntityExistsAsync(dbContext.Companies, companyId);
        
        bool codeExists = await dbContext.Divisions.AnyAsync(d => d.CompanyId == companyId && d.Code == request.Code);
        if (codeExists) throw new ValidationException("Division code already exists for the company.");

        DivisionEntity divisionEntity = new()
        {
            Name = request.Name,
            Code = request.Code,
            LeaderId = request.LeaderId,
            CompanyId = companyId
        };

        dbContext.Divisions.Add(divisionEntity);

        await dbContext.SaveChangesAsync();

        return divisionEntity.ToResponse();
    }

    public async Task DeleteAsync(int divisionId)
    {
        var division = await dbContext.Divisions.FirstOrDefaultAsync(d => d.Id == divisionId);
        if (division == null) throw new NotFoundException("Division not found.");
        dbContext.Divisions.Remove(division);
        await dbContext.SaveChangesAsync();
    }
    
    public async Task<DivisionResponse> UpdateAsync(int divisionId, UpdateOrganizationNodeRequest request)
    {
        DivisionEntity? division = await dbContext.Divisions.FirstOrDefaultAsync(d => d.Id == divisionId);
        if (division == null) throw new NotFoundException("Division not found.");

        division = await organizationNodeService.UpdateAsync(
            dbContext.Divisions,
            dbContext.Divisions.Where(d => d.CompanyId == division.CompanyId),
            divisionId,
            request
        );

        return division.ToResponse();
    }
    
    public async Task<EmployeeResponse> GetLeaderAsync(int divisionId)
    {
        return await organizationNodeService.GetLeaderAsync(dbContext.Divisions, divisionId);
    }

    public async Task<DivisionResponse> SetLeaderAsync(int divisionId, int leaderId)
    {
        DivisionEntity divisionEntity = await organizationNodeService.SetLeaderAsync(dbContext.Divisions, divisionId, leaderId);
        return divisionEntity.ToResponse();
    }

    public async Task<DivisionResponse> RemoveLeaderAsync(int divisionId)
    {
        DivisionEntity divisionEntity = await organizationNodeService.RemoveLeaderAsync(dbContext.Divisions, divisionId);
        return divisionEntity.ToResponse();
    }
}