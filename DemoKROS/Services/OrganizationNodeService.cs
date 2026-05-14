using DemoKROS.Data;
using DemoKROS.DTO.Common;
using DemoKROS.DTO.Employees;
using DemoKROS.Entities;
using Microsoft.EntityFrameworkCore;

namespace DemoKROS.Services;

public class OrganizationNodeService(AppDbContext dbContext)
{
    public async Task<ServiceResult<TEntity>> UpdateAsync<TEntity>(
        DbSet<TEntity> entities,
        IQueryable<TEntity> uniquenessScope,
        int entityId,
        UpdateOrganizationNodeRequest request) where TEntity : OrganizationNodeEntity
    {
        TEntity? entity = await entities.FirstOrDefaultAsync(e => e.Id == entityId);

        if (entity == null)
            return ServiceResult<TEntity>.NotFound($"{typeof(TEntity).Name} not found.");

        if (!string.IsNullOrWhiteSpace(request.Code))
        {
            bool codeExists = await uniquenessScope.AnyAsync(e => e.Code == request.Code && e.Id != entityId);

            if (codeExists)
                return ServiceResult<TEntity>.BadRequest($"{typeof(TEntity).Name} code already exists.");

            entity.Code = request.Code;
        }

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            entity.Name = request.Name;
        }

        await dbContext.SaveChangesAsync();

        return ServiceResult<TEntity>.Ok(entity);
    }
    
    public async Task<ServiceResult<EmployeeResponse>> GetLeaderAsync<TEntity>(DbSet<TEntity> entities, int entityId) where TEntity : OrganizationNodeEntity
    {
        TEntity? entity = await entities.Include(e => e.Leader).FirstOrDefaultAsync(e => e.Id == entityId);

        if (entity == null)
            return ServiceResult<EmployeeResponse>.NotFound($"{typeof(TEntity).Name} not found.");

        if (entity.Leader == null)
            return ServiceResult<EmployeeResponse>.NotFound($"{typeof(TEntity).Name} has no leader.");

        return ServiceResult<EmployeeResponse>.Ok(entity.Leader.ToResponse());
    }

    public async Task<ServiceResult<TEntity>> SetLeaderAsync<TEntity>(DbSet<TEntity> entities, int entityId, int leaderId) where TEntity : OrganizationNodeEntity
    {
        TEntity? entity = await entities.FirstOrDefaultAsync(e => e.Id == entityId);

        if (entity == null)
            return ServiceResult<TEntity>.NotFound($"{typeof(TEntity).Name} not found.");

        EmployeeEntity? leader = await dbContext.Employees.FirstOrDefaultAsync(e => e.Id == leaderId);

        if (leader == null)
            return ServiceResult<TEntity>.NotFound("Employee not found.");
        
        CompanyEntity? company = entity.GetCompany();

        if (company == null)
            return ServiceResult<TEntity>.BadRequest("Could not resolve company hierarchy.");

        bool isCompanyEmployee = company.Employees.Any(e => e.Id == leaderId);

        if (!isCompanyEmployee)
            return ServiceResult<TEntity>.BadRequest("Leader must be an employee of the same company.");

        entity.LeaderId = leaderId;

        await dbContext.SaveChangesAsync();

        return ServiceResult<TEntity>.Ok(entity);
    }

    public async Task<ServiceResult<TEntity>> RemoveLeaderAsync<TEntity>(DbSet<TEntity> entities, int entityId) where TEntity : OrganizationNodeEntity
    {
        TEntity? entity = await entities.FirstOrDefaultAsync(e => e.Id == entityId);

        if (entity == null)
            return ServiceResult<TEntity>.NotFound($"{typeof(TEntity).Name} not found.");

        entity.LeaderId = null;

        await dbContext.SaveChangesAsync();

        return ServiceResult<TEntity>.Ok(entity);
    }
}