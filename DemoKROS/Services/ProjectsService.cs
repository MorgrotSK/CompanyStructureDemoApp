using DemoKROS.Data;
using DemoKROS.DTO.Common;
using DemoKROS.DTO.Departments;
using DemoKROS.DTO.Employees;
using DemoKROS.DTO.Projects;
using DemoKROS.Entities;
using Microsoft.EntityFrameworkCore;

namespace DemoKROS.Services;

public class ProjectsService(AppDbContext dbContext, OrganizationNodeService organizationNodeService)
{
    public async Task<List<ProjectResponse>> GetAllAsync()
    {
        return await dbContext.Projects.Select(p => p.ToResponse()).ToListAsync();
    }

    public async Task<ServiceResult<ProjectResponse>> GetByIdAsync(int id)
    {
        ProjectEntity? project = await dbContext.Projects.FirstOrDefaultAsync(p => p.Id == id);

        if (project == null)
            return ServiceResult<ProjectResponse>.NotFound("Project not found.");

        return ServiceResult<ProjectResponse>.Ok(project.ToResponse());
    }

    public async Task<ServiceResult<List<DepartmentResponse>>> GetProjectDepartmentsAsync(int id)
    {
        if (!await DbValidationHelpers.EntityExistsAsync(dbContext.Projects, id))
            return ServiceResult<List<DepartmentResponse>>.NotFound("Project not found.");

        var departments = await dbContext.Departments
            .Where(d => d.ProjectId == id)
            .Select(d => d.ToResponse())
            .ToListAsync();

        return ServiceResult<List<DepartmentResponse>>.Ok(departments);
    }

    public async Task<ServiceResult<ProjectResponse>> CreateAsync(CreateProjectRequest request, int divisionId)
    {
        if (!await DbValidationHelpers.EntityExistsAsync(dbContext.Divisions, divisionId))
            return ServiceResult<ProjectResponse>.NotFound("Division not found.");

        bool codeExists = await dbContext.Projects.AnyAsync(p => p.DivisionId == divisionId && p.Code == request.Code);

        if (codeExists)
            return ServiceResult<ProjectResponse>.BadRequest("Project code already exists for the division.");

        ProjectEntity projectEntity = new()
        {
            Name = request.Name,
            Code = request.Code,
            DivisionId = divisionId
        };
        
        if (request.LeaderId is not null)
        {
            var leaderResult = await organizationNodeService.ValidateLeaderAsync(projectEntity, request.LeaderId.Value);

            if (!leaderResult.Success)
                return ServiceResult<ProjectResponse>.Fail(leaderResult);

            projectEntity.LeaderId = request.LeaderId.Value;
        }

        dbContext.Projects.Add(projectEntity);
        await dbContext.SaveChangesAsync();

        return ServiceResult<ProjectResponse>.Ok(projectEntity.ToResponse());
    }

    public async Task<ServiceResult> DeleteAsync(int id)
    {
        ProjectEntity? project = await dbContext.Projects.FirstOrDefaultAsync(p => p.Id == id);

        if (project == null)
            return ServiceResult.NotFound("Project not found.");

        dbContext.Projects.Remove(project);
        await dbContext.SaveChangesAsync();

        return ServiceResult.NoContent();
    }

    public async Task<ServiceResult<ProjectResponse>> UpdateAsync(int projectId, UpdateOrganizationNodeRequest request)
    {
        ProjectEntity? project = await dbContext.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

        if (project == null)
            return ServiceResult<ProjectResponse>.NotFound("Project not found.");

        var result = await organizationNodeService.UpdateAsync(
            dbContext.Projects,
            dbContext.Projects.Where(p => p.DivisionId == project.DivisionId),
            projectId,
            request);

        if (!result.Success)
            return ServiceResult<ProjectResponse>.Fail(result);

        return ServiceResult<ProjectResponse>.Ok(result.Data!.ToResponse());
    }

    public async Task<ServiceResult<EmployeeResponse>> GetLeaderAsync(int projectId)
    {
        return await organizationNodeService.GetLeaderAsync(dbContext.Projects, projectId);
    }

    public async Task<ServiceResult<ProjectResponse>> SetLeaderAsync(int projectId, int leaderId)
    {
        var result = await organizationNodeService.SetLeaderAsync(dbContext.Projects, projectId, leaderId);

        if (!result.Success)
            return ServiceResult<ProjectResponse>.Fail(result);

        return ServiceResult<ProjectResponse>.Ok(result.Data!.ToResponse());
    }

    public async Task<ServiceResult<ProjectResponse>> RemoveLeaderAsync(int projectId)
    {
        var result = await organizationNodeService.RemoveLeaderAsync(dbContext.Projects, projectId);

        if (!result.Success)
            return ServiceResult<ProjectResponse>.Fail(result);

        return ServiceResult<ProjectResponse>.Ok(result.Data!.ToResponse());
    }
}