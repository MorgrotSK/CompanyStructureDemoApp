using DemoKROS.Data;
using DemoKROS.DTO.Common;
using DemoKROS.DTO.Employees;
using DemoKROS.Entities;
using Microsoft.EntityFrameworkCore;

namespace DemoKROS.Services;

public class EmployeesService(AppDbContext dbContext)
{
    public async Task<List<EmployeeResponse>> GetAllAsync()
    {
        return await dbContext.Employees.Select(e => e.ToResponse()).ToListAsync();
    }

    public async Task<ServiceResult<EmployeeResponse>> GetByIdAsync(int id)
    {
        EmployeeEntity? employee = await dbContext.Employees.FirstOrDefaultAsync(e => e.Id == id);

        if (employee == null) return ServiceResult<EmployeeResponse>.NotFound("Employee not found.");

        return ServiceResult<EmployeeResponse>.Ok(employee.ToResponse());
    }

    public async Task<ServiceResult<EmployeeResponse>> CreateAsync(CreateEmployeeRequest request, int companyId)
    {
        if (!await DbValidationHelpers.EntityExistsAsync(dbContext.Companies, companyId))
            return ServiceResult<EmployeeResponse>.NotFound("Company not found.");

        EmployeeEntity employeeEntity = new()
        {
            Title = request.Title ?? String.Empty,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Phone = request.Phone,
            Email = request.Email,
            CompanyId = companyId
        };

        dbContext.Employees.Add(employeeEntity);
        await dbContext.SaveChangesAsync();

        return ServiceResult<EmployeeResponse>.Ok(employeeEntity.ToResponse());
    }
    
    public async Task<ServiceResult<EmployeeResponse>> UpdateAsync(int employeeId, UpdateEmployeeRequest request)
    {
        EmployeeEntity? employee = await dbContext.Employees.FirstOrDefaultAsync(e => e.Id == employeeId);

        if (employee == null) return ServiceResult<EmployeeResponse>.NotFound("Employee not found.");

        if (request.Title != null) employee.Title = request.Title;
        if (request.FirstName != null) employee.FirstName = request.FirstName;
        if (request.LastName != null) employee.LastName = request.LastName;
        if (request.Phone != null) employee.Phone = request.Phone;
        if (request.Email != null) employee.Email = request.Email;

        await dbContext.SaveChangesAsync();

        return ServiceResult<EmployeeResponse>.Ok(employee.ToResponse());
    }

    public async Task<ServiceResult> DeleteAsync(int id)
    {
        EmployeeEntity? employee = await dbContext.Employees.FirstOrDefaultAsync(e => e.Id == id);

        if (employee == null) return ServiceResult.NotFound("Employee not found.");

        if (await DbValidationHelpers.EmployeeIsLeaderAsync(dbContext, id))
            return ServiceResult.BadRequest("Employee cannot be deleted because they are assigned as a leader.");

        dbContext.Employees.Remove(employee);
        await dbContext.SaveChangesAsync();

        return ServiceResult.NoContent();
    }
}