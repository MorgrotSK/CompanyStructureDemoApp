using System.ComponentModel.DataAnnotations;
using DemoKROS.Data;
using DemoKROS.DTO.Common;
using DemoKROS.DTO.Company;
using DemoKROS.DTO.Departments;
using DemoKROS.DTO.Employees;
using DemoKROS.Entities;
using DemoKROS.Exceptions;
using Microsoft.EntityFrameworkCore;
using ValidationException = DemoKROS.Exceptions.ValidationException;

namespace DemoKROS.Services;

public class DepartmentsService(AppDbContext dbContext, OrganizationNodeService organizationNodeService)
{
    public async Task<List<DepartmentResponse>> GetAllAsync()
    {
        return await dbContext.Departments.Select(d => d.ToResponse()).ToListAsync();
    }
    
    public async Task<DepartmentResponse> GetByIdAsync(int id)
    {
        DepartmentEntity? department = await dbContext.Departments.FirstOrDefaultAsync(d => d.Id == id);

        if (department == null)
        {
            throw new NotFoundException("Department not found.");
        }

        return department.ToResponse();
    }
    
    public async Task<DepartmentResponse> CreateAsync(CreateDepartmentRequest request, int projectId) {
        await DbValidationHelpers.EnsureEntityExistsAsync(dbContext.Projects, projectId);

        bool codeExists = await dbContext.Departments.AnyAsync(d => d.ProjectId == projectId && d.Code == request.Code);

        if (codeExists)
        {
            throw new ValidationException("Department code already exists for the project.");
        }

        DepartmentEntity departmentEntity = new()
        {
            Name = request.Name,
            Code = request.Code,
            ProjectId = projectId
        };

        dbContext.Departments.Add(departmentEntity);

        await dbContext.SaveChangesAsync();
        
        if (request.LeaderId is not null)
        {
            departmentEntity = await organizationNodeService.SetLeaderAsync(dbContext.Departments, departmentEntity.Id, request.LeaderId.Value);

        }

        return departmentEntity.ToResponse();
    }
    
    public async Task<DepartmentResponse> UpdateAsync(int departmentId, UpdateOrganizationNodeRequest request)
    {
        DepartmentEntity? department = await dbContext.Departments.FirstOrDefaultAsync(d => d.Id == departmentId);
        if (department == null) throw new NotFoundException("Department not found.");

        var department1 = department;
        department = await organizationNodeService.UpdateAsync(
            dbContext.Departments,
            dbContext.Departments.Where(d => d.ProjectId == department1.ProjectId),
            departmentId,
            request
        );

        return department.ToResponse();
    }
    
    public async Task DeleteAsync(int departmentId)
    {
        DepartmentEntity? department = await dbContext.Departments.FirstOrDefaultAsync(d => d.Id == departmentId);

        if (department == null) throw new NotFoundException("Department not found.");

        dbContext.Departments.Remove(department);

        await dbContext.SaveChangesAsync();
    }
    
    public async Task<EmployeeResponse> GetLeaderAsync(int departmentId)
    {
        return await organizationNodeService.GetLeaderAsync(dbContext.Departments, departmentId);
    }

    public async Task<DepartmentResponse> SetLeaderAsync(int departmentId, int leaderId)
    {
        DepartmentEntity departmentEntity = await organizationNodeService.SetLeaderAsync(dbContext.Departments, departmentId, leaderId);
        return departmentEntity.ToResponse();
    }

    public async Task<DepartmentResponse> RemoveLeaderAsync(int departmentId)
    {
        DepartmentEntity departmentEntity = await organizationNodeService.RemoveLeaderAsync(dbContext.Departments, departmentId);
        return departmentEntity.ToResponse();
    }
}