using DemoKROS.Data;
using DemoKROS.DTO.Common;
using DemoKROS.DTO.Departments;
using DemoKROS.DTO.Employees;
using DemoKROS.Entities;
using Microsoft.EntityFrameworkCore;

namespace DemoKROS.Services;

public class DepartmentsService(AppDbContext dbContext, OrganizationNodeService organizationNodeService)
{
    public async Task<List<DepartmentResponse>> GetAllAsync()
    {
        return await dbContext.Departments.Select(d => d.ToResponse()).ToListAsync();
    }
    
    public async Task<ServiceResult<DepartmentResponse>> GetByIdAsync(int id)
    {
        DepartmentEntity? department = await dbContext.Departments.FirstOrDefaultAsync(d => d.Id == id);

        if (department == null) return ServiceResult<DepartmentResponse>.NotFound("Department not found.");

        return ServiceResult<DepartmentResponse>.Ok(department.ToResponse());
    }
    
    public async Task<ServiceResult<DepartmentResponse>> CreateAsync(CreateDepartmentRequest request, int projectId)
    {
        if (!await DbValidationHelpers.EntityExistsAsync(dbContext.Projects, projectId))
            return ServiceResult<DepartmentResponse>.NotFound("Project not found.");

        bool codeExists = await dbContext.Departments.AnyAsync(d => d.ProjectId == projectId && d.Code == request.Code);

        if (codeExists) return ServiceResult<DepartmentResponse>.BadRequest("Department code already exists for the project.");

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
            var leaderResult = await organizationNodeService.SetLeaderAsync(dbContext.Departments, departmentEntity.Id, request.LeaderId.Value);

            if (!leaderResult.Success)
                return new ServiceResult<DepartmentResponse> { Success = false, Error = leaderResult.Error, StatusCode = leaderResult.StatusCode };

            departmentEntity = leaderResult.Data!;
        }

        return ServiceResult<DepartmentResponse>.Ok(departmentEntity.ToResponse());
    }
    
    public async Task<ServiceResult<DepartmentResponse>> UpdateAsync(int departmentId, UpdateOrganizationNodeRequest request)
    {
        DepartmentEntity? department = await dbContext.Departments.FirstOrDefaultAsync(d => d.Id == departmentId);

        if (department == null) return ServiceResult<DepartmentResponse>.NotFound("Department not found.");

        var result = await organizationNodeService.UpdateAsync(dbContext.Departments, dbContext.Departments.Where(d => d.ProjectId == department.ProjectId), departmentId, request);

        if (!result.Success)
            return new ServiceResult<DepartmentResponse> { Success = false, Error = result.Error, StatusCode = result.StatusCode };

        return ServiceResult<DepartmentResponse>.Ok(result.Data!.ToResponse());
    }
    
    public async Task<ServiceResult> DeleteAsync(int departmentId)
    {
        DepartmentEntity? department = await dbContext.Departments.FirstOrDefaultAsync(d => d.Id == departmentId);

        if (department == null) return ServiceResult.NotFound("Department not found.");

        dbContext.Departments.Remove(department);
        await dbContext.SaveChangesAsync();

        return ServiceResult.Ok();
    }
    
    public async Task<ServiceResult<EmployeeResponse>> GetLeaderAsync(int departmentId)
    {
        return await organizationNodeService.GetLeaderAsync(dbContext.Departments, departmentId);
    }

    public async Task<ServiceResult<DepartmentResponse>> SetLeaderAsync(int departmentId, int leaderId)
    {
        var result = await organizationNodeService.SetLeaderAsync(dbContext.Departments, departmentId, leaderId);

        if (!result.Success)
            return new ServiceResult<DepartmentResponse> { Success = false, Error = result.Error, StatusCode = result.StatusCode };

        return ServiceResult<DepartmentResponse>.Ok(result.Data!.ToResponse());
    }

    public async Task<ServiceResult<DepartmentResponse>> RemoveLeaderAsync(int departmentId)
    {
        var result = await organizationNodeService.RemoveLeaderAsync(dbContext.Departments, departmentId);

        if (!result.Success)
            return new ServiceResult<DepartmentResponse> { Success = false, Error = result.Error, StatusCode = result.StatusCode };

        return ServiceResult<DepartmentResponse>.Ok(result.Data!.ToResponse());
    }
}