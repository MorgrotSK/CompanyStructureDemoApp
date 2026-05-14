using DemoKROS.Data;
using DemoKROS.DTO.Employees;
using DemoKROS.Entities;
using DemoKROS.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DemoKROS.Services;

public class EmployeesService(AppDbContext dbContext)
{
    public async Task<List<EmployeeResponse>> GetAllAsync()
    {
        return await dbContext.Employees.Select(e => e.ToResponse()).ToListAsync();
    }

    public async Task<EmployeeResponse> GetByIdAsync(int id)
    {
        EmployeeEntity? employee = await dbContext.Employees.FirstOrDefaultAsync(e => e.Id == id);

        if (employee == null)
            throw new NotFoundException("Employee not found.");

        return employee.ToResponse();
    }

    public async Task<EmployeeResponse> CreateAsync(CreateEmployeeRequest request, int companyId)
    {
        await DbValidationHelpers.EnsureEntityExistsAsync(dbContext.Companies, companyId);

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

        return employeeEntity.ToResponse();
    }
    
    public async Task<EmployeeResponse> UpdateAsync(int employeeId, UpdateEmployeeRequest request)
    {
        EmployeeEntity? employee = await dbContext.Employees.FirstOrDefaultAsync(e => e.Id == employeeId);

        if (employee == null)
            throw new NotFoundException("Employee not found.");

        if (request.Title != null) employee.Title = request.Title;

        if (request.FirstName != null) employee.FirstName = request.FirstName;

        if (request.LastName != null) employee.LastName = request.LastName;

        if (request.Phone != null) employee.Phone = request.Phone;

        if (request.Email != null) employee.Email = request.Email;

        await dbContext.SaveChangesAsync();

        return employee.ToResponse();
    }

    public async Task DeleteAsync(int id)
    {
        EmployeeEntity? employee = await dbContext.Employees.FirstOrDefaultAsync(e => e.Id == id);

        if (employee == null) throw new NotFoundException("Employee not found.");

        dbContext.Employees.Remove(employee);

        await dbContext.SaveChangesAsync();
    }
}