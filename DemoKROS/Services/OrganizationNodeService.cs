using DemoKROS.Data;
using DemoKROS.DTO.Common;
using DemoKROS.DTO.Employees;
using DemoKROS.Entities;
using DemoKROS.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DemoKROS.Services;

public class OrganizationNodeService(AppDbContext dbContext)
{
    public async Task<TEntity> UpdateAsync<TEntity>(
        DbSet<TEntity> entities,
        IQueryable<TEntity> uniquenessScope,
        int entityId,
        UpdateOrganizationNodeRequest request) where TEntity : OrganizationNodeEntity
    {
        TEntity? entity = await entities.FirstOrDefaultAsync(e => e.Id == entityId);
        if (entity == null) throw new NotFoundException($"{typeof(TEntity).Name} not found.");

        if (!string.IsNullOrWhiteSpace(request.Code))
        {
            bool codeExists = await uniquenessScope.AnyAsync(e => e.Code == request.Code && e.Id != entityId);

            if (codeExists)
                throw new ValidationException($"{typeof(TEntity).Name} code already exists.");

            entity.Code = request.Code;
        }

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            entity.Name = request.Name;
        }

        await dbContext.SaveChangesAsync();

        return entity;
    }
    
    public async Task<EmployeeResponse> GetLeaderAsync<TEntity>(DbSet<TEntity> entities, int entityId) where TEntity : OrganizationNodeEntity
    {
        TEntity? entity = await entities
            .Include(e => e.Leader)
            .FirstOrDefaultAsync(e => e.Id == entityId);

        if (entity == null) throw new NotFoundException($"{typeof(TEntity).Name} not found.");
        if (entity.Leader == null) throw new NotFoundException($"{typeof(TEntity).Name} has no leader.");

        return entity.Leader.ToResponse();
    }
    public async Task<TEntity> SetLeaderAsync<TEntity>(DbSet<TEntity> entities, int entityId, int leaderId) where TEntity : OrganizationNodeEntity
    {
        TEntity? entity = await entities.FirstOrDefaultAsync(e => e.Id == entityId);
        if (entity == null) throw new NotFoundException($"{typeof(TEntity).Name} not found.");

        await DbValidationHelpers.EnsureEntityExistsAsync(dbContext.Employees, leaderId);
        
        CompanyEntity company = entity.GetCompany() ?? throw new Exception("Could not resolve company hierarchy.");

        bool isCompanyEmployee = company.Employees.Any(e => e.Id == leaderId);

        if (!isCompanyEmployee) throw new ValidationException("Leader must be an employee of the same company.");

        entity.LeaderId = leaderId;

        await dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task<TEntity> RemoveLeaderAsync<TEntity>(DbSet<TEntity> entities, int entityId) where TEntity : OrganizationNodeEntity
    {
        TEntity? entity = await entities.FirstOrDefaultAsync(e => e.Id == entityId);
        if (entity == null) throw new NotFoundException($"{typeof(TEntity).Name} not found.");

        entity.LeaderId = null;

        await dbContext.SaveChangesAsync();

        return entity;
    }
}