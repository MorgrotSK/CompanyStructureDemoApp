using DemoKROS.Data;
using DemoKROS.Handlers;
using DemoKROS.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddProblemDetails();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseLazyLoadingProxies()
        .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));builder.Services.AddScoped<OrganizationNodeService>();
builder.Services.AddScoped<CompaniesService>();
builder.Services.AddScoped<DivisionsService>();
builder.Services.AddScoped<EmployeesService>();
builder.Services.AddScoped<ProjectsService>();
builder.Services.AddScoped<DepartmentsService>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();

app.UseHttpsRedirection();
app.UseExceptionHandler();
app.MapControllers();
app.Run();