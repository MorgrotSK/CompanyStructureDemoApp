using DemoKROS.Data;
using DemoKROS.DTO.Common;
using DemoKROS.DTO.Company;
using DemoKROS.DTO.Divisions;
using DemoKROS.DTO.Employees;
using DemoKROS.Entities;
using DemoKROS.Exceptions;
using Microsoft.EntityFrameworkCore;
namespace DemoKROS.Services;


public class CompaniesService(AppDbContext dbContext, OrganizationNodeService organizationNodeService)
{
    public async Task<List<CompanyResponse>> GetAllAsync()
    {
        return await dbContext.Companies.Select(c => c.ToResponse()).ToListAsync();
    }

    public async Task<CompanyResponse> GetByIdAsync(int id)
    {
        CompanyEntity? company = await dbContext.Companies.FirstOrDefaultAsync(c => c.Id == id);
        if (company == null) throw new NotFoundException("Company not found.");
        return company.ToResponse();
    }
    
    public async Task<List<EmployeeResponse>> GetEmployeesAsync(int companyId)
    {
        await DbValidationHelpers.EnsureEntityExistsAsync(dbContext.Companies, companyId);

        return await dbContext.Employees
            .Where(e => e.CompanyId == companyId)
            .Select(e => e.ToResponse())
            .ToListAsync();
    }
    
    public async Task<List<DivisionResponse>> GetDivisionsAsync(int companyId)
    {
        await DbValidationHelpers.EnsureEntityExistsAsync(dbContext.Companies, companyId);

        return await dbContext.Divisions
            .Where(d => d.CompanyId == companyId)
            .Select(d => d.ToResponse())
            .ToListAsync();
    }

    public async Task<CompanyResponse> UpdateAsync(int companyId, UpdateOrganizationNodeRequest request)
    {
        CompanyEntity companyEntity = await organizationNodeService.UpdateAsync(dbContext.Companies, dbContext.Companies, companyId, request);
        return companyEntity.ToResponse();
    }

    public async Task<CompanyResponse> CreateAsync(CreateCompanyRequest request)
    {
        Console.WriteLine($"Received request: Name={request.Name}, Code={request.Code}");
        bool codeExists = await dbContext.Companies.AnyAsync(c => c.Code == request.Code);
        if (codeExists) throw new ValidationException("Company code already exists.");

        CompanyEntity companyEntity = new()
        {
            Name = request.Name,
            Code = request.Code,
        };

        dbContext.Companies.Add(companyEntity);

        await dbContext.SaveChangesAsync();

        return companyEntity.ToResponse();
    }
    
    public async Task DeleteAsync(int companyId)
    {
        CompanyEntity? company = await dbContext.Companies.FirstOrDefaultAsync(c => c.Id == companyId);
        if (company == null) throw new NotFoundException("Company not found.");

        dbContext.Companies.Remove(company);
        await dbContext.SaveChangesAsync();
    }
    
    public async Task<EmployeeResponse> GetLeaderAsync(int companyId)
    {
        return await organizationNodeService.GetLeaderAsync(dbContext.Companies, companyId);
    }
    
    public async Task<CompanyResponse> SetLeaderAsync(int companyId, int leaderId)
    {
        CompanyEntity companyEntity = await organizationNodeService.SetLeaderAsync(dbContext.Companies, companyId, leaderId);
        return companyEntity.ToResponse();
    }

    public async Task<CompanyResponse> RemoveLeaderAsync(int companyId)
    {
        CompanyEntity companyEntity = await organizationNodeService.RemoveLeaderAsync(dbContext.Companies, companyId);
        return companyEntity.ToResponse();
    }
}