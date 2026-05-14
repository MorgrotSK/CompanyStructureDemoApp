using DemoKROS.Entities;
using Microsoft.EntityFrameworkCore;

namespace DemoKROS.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<CompanyEntity> Companies => Set<CompanyEntity>();
    public DbSet<DivisionEntity> Divisions => Set<DivisionEntity>();
    public DbSet<ProjectEntity> Projects => Set<ProjectEntity>();
    public DbSet<DepartmentEntity> Departments => Set<DepartmentEntity>();
    public DbSet<EmployeeEntity> Employees => Set<EmployeeEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CompanyEntity>().HasIndex(c => c.Code).IsUnique();

        modelBuilder.Entity<DivisionEntity>().HasIndex(d => new { d.CompanyId, d.Code }).IsUnique();

        modelBuilder.Entity<ProjectEntity>().HasIndex(p => new { p.DivisionId, p.Code }).IsUnique();

        modelBuilder.Entity<DepartmentEntity>().HasIndex(d => new { d.ProjectId, d.Code }).IsUnique();
        
        ConfigureOrganizationNode<CompanyEntity>(modelBuilder);
        ConfigureOrganizationNode<DivisionEntity>(modelBuilder);    
        ConfigureOrganizationNode<ProjectEntity>(modelBuilder);
        ConfigureOrganizationNode<DepartmentEntity>(modelBuilder);
    }
    
    private void ConfigureOrganizationNode<T>(ModelBuilder modelBuilder) where T : OrganizationNodeEntity
    {
        modelBuilder.Entity<T>()
            .HasOne(o => o.Leader)
            .WithMany()
            .HasForeignKey(o => o.LeaderId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}