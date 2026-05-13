using DemoKROS.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DemoKROS.Data;

public static class DbValidationHelpers
{
    public static async Task EnsureEntityExistsAsync<TEntity>(DbSet<TEntity> dbSet, int id) where TEntity : class
    {
        bool exists = await dbSet.AnyAsync(e => EF.Property<int>(e, "Id") == id);
        if (!exists)
        {
            throw new NotFoundException($"{typeof(TEntity).Name} not found.");
        }
    }
}