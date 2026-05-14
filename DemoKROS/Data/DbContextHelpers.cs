using Microsoft.EntityFrameworkCore;

namespace DemoKROS.Data;

public static class DbValidationHelpers
{
    public static async Task<bool> EntityExistsAsync<TEntity>(DbSet<TEntity> dbSet, int id) where TEntity : class
    {
        return await dbSet.AnyAsync(e => EF.Property<int>(e, "Id") == id);
    }
    
    public static async Task<bool> EmployeeIsLeaderAsync(AppDbContext dbContext, int employeeId)
    {
        return await dbContext.Companies.AnyAsync(c => c.LeaderId == employeeId)
               || await dbContext.Divisions.AnyAsync(d => d.LeaderId == employeeId)
               || await dbContext.Projects.AnyAsync(p => p.LeaderId == employeeId)
               || await dbContext.Departments.AnyAsync(d => d.LeaderId == employeeId);
    }
}