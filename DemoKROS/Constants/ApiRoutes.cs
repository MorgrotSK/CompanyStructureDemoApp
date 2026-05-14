namespace DemoKROS.Constants;

public static class ApiRoutes
{
    public const string Companies = "api/companies";
    public const string Divisions = "api/divisions";
    public const string Projects = "api/projects";
    public const string Departments = "api/departments";
    public const string Employees = "api/employees";

    public static class RouteNames
    {
        public const string GetCompanyById = "GetCompanyById";
        public const string GetDivisionById = "GetDivisionById";
        public const string GetProjectById = "GetProjectById";
        public const string GetDepartmentById = "GetDepartmentById";
        public const string GetEmployeeById = "GetEmployeeById";
    }

    public static class CompaniesRoutes
    {
        public const string ById = "{companyId:int}";
        public const string Employees = "{companyId:int}/employees";
        public const string Divisions = "{companyId:int}/divisions";
        public const string Leader = "{companyId:int}/leader";
        public const string LeaderById = "{companyId:int}/leader/{leaderId:int}";
    }

    public static class DivisionsRoutes
    {
        public const string ById = "{divisionId:int}";
        public const string Projects = "{divisionId:int}/projects";
        public const string Leader = "{divisionId:int}/leader";
        public const string LeaderById = "{divisionId:int}/leader/{leaderId:int}";
    }

    public static class ProjectsRoutes
    {
        public const string ById = "{projectId:int}";
        public const string Departments = "{projectId:int}/departments";
        public const string Leader = "{projectId:int}/leader";
        public const string LeaderById = "{projectId:int}/leader/{leaderId:int}";
    }

    public static class DepartmentsRoutes
    {
        public const string ById = "{departmentId:int}";
        public const string Leader = "{departmentId:int}/leader";
        public const string LeaderById = "{departmentId:int}/leader/{leaderId:int}";
    }

    public static class EmployeesRoutes
    {
        public const string ById = "{employeeId:int}";
    }
}