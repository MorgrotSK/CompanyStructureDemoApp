using DemoKROS.Data;
using DemoKROS.DTO.Common;
using DemoKROS.DTO.Company;
using DemoKROS.DTO.Departments;
using DemoKROS.DTO.Employees;
using DemoKROS.DTO.Projects;
using DemoKROS.Entities;
using DemoKROS.Exceptions;
using Microsoft.EntityFrameworkCore;
using ValidationException = DemoKROS.Exceptions.ValidationException;

namespace DemoKROS.Services;

public class ProjectsService(AppDbContext dbContext, OrganizationNodeService organizationNodeService)
{
    public async Task<List<ProjectResponse>> GetAllAsync()
    {
        return await dbContext.Projects
            .Select(p => p.ToResponse())
            .ToListAsync();
    }
    
    public async Task<ProjectResponse> GetByIdAsync(int id)
    {
        ProjectEntity? project = await dbContext.Projects
            .FirstOrDefaultAsync(p => p.Id == id);

        if (project == null)
        {
            throw new NotFoundException("Project not found.");
        }

        return project.ToResponse();
    }
    
    public async Task<List<DepartmentResponse>> GetProjectDepartmentsAsync(int id)
    {
        await DbValidationHelpers.EnsureEntityExistsAsync(dbContext.Projects, id);

        return await dbContext.Departments
            .Where(d => d.ProjectId == id)
            .Select(d => d.ToResponse())
            .ToListAsync();
    }
    
    public async Task<ProjectResponse> CreateAsync(CreateProjectRequest request, int divisionId)
    {
        await DbValidationHelpers.EnsureEntityExistsAsync(dbContext.Divisions, divisionId);
        
        bool codeExists = await dbContext.Projects.AnyAsync(p => p.DivisionId == divisionId && p.Code == request.Code);

        if (codeExists)
        {
            throw new ValidationException("Project code already exists for the division.");
        }

        ProjectEntity projectEntity = new()
        {
            Name = request.Name,
            Code = request.Code,
            DivisionId = divisionId
        };

        dbContext.Projects.Add(projectEntity);

        await dbContext.SaveChangesAsync();
        
        if (request.LeaderId is not null)
        {
            projectEntity = await organizationNodeService.SetLeaderAsync(dbContext.Projects, projectEntity.Id, request.LeaderId.Value);

        }

        return projectEntity.ToResponse();
    }
    
    public async Task DeleteAsync(int id)
    {
        ProjectEntity? project = await dbContext.Projects.FirstOrDefaultAsync(p => p.Id == id);

        if (project == null)
        {
            throw new NotFoundException("Project not found.");
        }
        dbContext.Projects.Remove(project);
        await dbContext.SaveChangesAsync();
    }
    
    public async Task<ProjectResponse> UpdateAsync(int projectId, UpdateOrganizationNodeRequest request)
    {
        ProjectEntity? project = await dbContext.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
        if (project == null) throw new NotFoundException("Project not found.");

        project = await organizationNodeService.UpdateAsync(
            dbContext.Projects,
            dbContext.Projects.Where(p => p.DivisionId == project.DivisionId),
            projectId,
            request
        );

        return project.ToResponse();
    }
    
    public async Task<EmployeeResponse> GetLeaderAsync(int projectId)
    {
        return await organizationNodeService.GetLeaderAsync(dbContext.Projects, projectId);
    }

    public async Task<ProjectResponse> SetLeaderAsync(int projectId, int leaderId)
    {
        ProjectEntity projectEntity = await organizationNodeService.SetLeaderAsync(dbContext.Projects, projectId, leaderId);
        return projectEntity.ToResponse();
    }

    public async Task<ProjectResponse> RemoveLeaderAsync(int projectId)
    {
        ProjectEntity projectEntity = await organizationNodeService.RemoveLeaderAsync(dbContext.Projects, projectId);
        return projectEntity.ToResponse();
    }
}