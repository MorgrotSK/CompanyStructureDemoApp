# DemoKROS

REST API for managing hierarchical company organizational structures.

## Technologies

- .NET 9
- ASP.NET Core Web API
- Entity Framework Core 9
- Entity Framework Core 9 Design
- Entity Framework Core 9 SqlServer
- Entity Framework Core 9 Tools
- Microsoft SQL Server
- Scalar OpenAPI
- TeaPie

## Setup

### 1. Configure database connection

Edit `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=DemoKROS;Trusted_Connection=True;TrustServerCertificate=True"
}
```

### 2. Create database

Execute the provided `script.sql` script on the database configured in `appsettings.json` connection string.

Alternative using Entity Framework migrations:

#### Install Entity Framework CLI tools

```bash
dotnet tool install --global dotnet-ef
```

#### Apply migrations

```bash
dotnet ef database update
```

### 4. Run application

```bash
dotnet run
```

## OpenAPI / Scalar

Scalar API documentation is available at:

```text
/scalar
```

Example:

```text
https://localhost:xxxx/scalar
```

## Notes
- DTO validation is implemented using data annotations.
- Global exception handling is implemented using `IExceptionHandler`.