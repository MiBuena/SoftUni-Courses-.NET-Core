using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            var optionBuilder = new DbContextOptionsBuilder<SoftUniContext>();

            optionBuilder.UseSqlServer("Server=.;Database=SoftUni;Integrated Security=True;");

            var context = new SoftUniContext();

            using (context)
            {

                var projects = GetAddressesByTown(context);

                Console.WriteLine(projects);
            }
        }

        public static string RemoveTown(SoftUniContext context)
        {
            var town = context.Towns.Include(x=>x.Addresses).FirstOrDefault(x => x.Name == "Seattle");

            var employees = context.Employees
                .Where(x => x.Address.Town.Name == "Seattle")
                .ToList();


            foreach (var employee in employees)
            {
                employee.AddressId = null;
            }

            var addressesCount = town.Addresses.Count;

            context.Addresses.RemoveRange(town.Addresses);

            context.Towns.Remove(town);

            context.SaveChanges();

            var result = $"{addressesCount} addresses in Seattle were deleted";

            return result;
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(x => x.FirstName.StartsWith("Sa"))
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    x.JobTitle,
                    x.Salary
                })
                .OrderBy(x => x.FirstName)
                .ThenBy(y => y.LastName)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle} - (${employee.Salary:F2})");
            }

            return sb.ToString().TrimEnd();
        }

        public static string DeleteProjectById(SoftUniContext context)
        {
            var project = context.Projects.Include(x=>x.EmployeesProjects).FirstOrDefault(x=>x.ProjectId == 2);

            context.EmployeesProjects.RemoveRange(project.EmployeesProjects);

            context.Projects.Remove(project);

            context.SaveChanges();


            var projectNames = context.Projects
                .Take(10)
                .Select(x => x.Name)
                .ToList();

            StringBuilder sb = new StringBuilder();


            foreach (var name in projectNames)
            {
                sb.AppendLine($"{name}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetLatestProjects(SoftUniContext context)
        {
            var projects = context.Projects
                .OrderByDescending(x => x.StartDate)
                .Take(10)
                .OrderBy(x => x.Name)
                .Select(x =>
                new
                {
                    x.Name,
                    x.Description,
                    x.StartDate
                }).ToList();


            StringBuilder sb = new StringBuilder();

            foreach (var project in projects)
            {
                sb.AppendLine($"{project.Name}");
                sb.AppendLine($"{project.Description}");
                sb.AppendLine($"{project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string IncreaseSalaries(SoftUniContext context)
        {
            var departments = new[]
            {
                "Engineering",
                "Tool Design",
                "Marketing",
                "Information Services"
            };

            var employees = context.Employees
                .Where(x => departments.Contains(x.Department.Name))
                .ToList();

            foreach (var employee in employees)
            {
                employee.Salary *= 1.12M;
            }

            context.SaveChanges();


            var orderedEmployees = employees
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName);


            StringBuilder sb = new StringBuilder();

            foreach (var employee in orderedEmployees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} (${employee.Salary:F2})");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var departments = context.Departments
                .Where(x => x.Employees.Count > 5)
                .OrderBy(y => y.Employees.Count)
                .ThenBy(z => z.Name)
                .Select(x => new
                {
                    x.Name,
                    ManagersFirstName = x.Manager.FirstName,
                    ManagersLastName = x.Manager.LastName,
                    EmployeesData = x.Employees
                    .OrderBy(z => z.FirstName)
                    .OrderBy(z => z.LastName)
                    .Select(y =>
                    new
                    {
                        y.FirstName,
                        y.LastName,
                        y.JobTitle
                    }).ToList()
                })
                .ToList();


            StringBuilder sb = new StringBuilder();

            foreach (var department in departments)
            {
                sb.AppendLine($"{department.Name} - {department.ManagersFirstName} {department.ManagersLastName}");

                foreach (var employee in department.EmployeesData)
                {
                    sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployee147(SoftUniContext context)
        {
            var employee = context.Employees
                .Select(y => new
                {
                    y.FirstName,
                    y.LastName,
                    y.JobTitle,
                    y.EmployeeId,
                    Projects = y.EmployeesProjects

                .Select(m => new
                {
                    ProjectName = m.Project.Name
                }).ToList()
                })
                .FirstOrDefault(x => x.EmployeeId == 147);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");

            foreach (var projectName in employee.Projects)
            {
                sb.AppendLine(projectName.ProjectName);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            var addresses = context.Addresses
                .OrderByDescending(x => x.Employees.Count)
                .ThenBy(x => x.Town.Name)
                .ThenBy(x => x.AddressText)
                .Take(10)
                .Select(x =>
                new
                {
                    AddressText = x.AddressText,
                    TownName = x.Town.Name,
                    EmployeeCount = x.Employees.Count
                })
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var address in addresses)
            {
                sb.AppendLine($"{address.AddressText}, {address.TownName} - {address.EmployeeCount} employees");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(x => x.EmployeesProjects
                .Any(y => y.Project.StartDate.Year >= 2001 && y.Project.StartDate.Year <= 2003))
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    ManagerFirstName = x.Manager.FirstName,
                    ManagerLastName = x.Manager.LastName,
                    Projects = x.EmployeesProjects.Select(y => new
                    {
                        ProjectName = y.Project.Name,
                        StartDate = y.Project.StartDate,
                        EndDate = y.Project.EndDate
                    }).ToList()
                })
                .Take(10)
                .ToList();



            StringBuilder result = new StringBuilder();

            foreach (var employee in employees)
            {
                result.AppendLine($"{employee.FirstName} {employee.LastName}" +
                    $" - Manager: {employee.ManagerFirstName} {employee.ManagerLastName}");

                foreach (var project in employee.Projects)
                {

                    var endDateValue = project.EndDate.HasValue ? project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) : "not finished";

                    result.AppendLine($"--{project.ProjectName} - {project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)} - {endDateValue}");
                }
            }

            return result.ToString().TrimEnd();
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            var address = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            var employee = context.Employees.FirstOrDefault(x => x.LastName == "Nakov");

            employee.Address = address;

            context.SaveChanges();

            var textAddresses = context.Employees
                .OrderByDescending(x => x.AddressId)
                .Take(10)
                .Select(x => x.Address.AddressText)
                .ToList();

            var result = string.Join(Environment.NewLine, textAddresses);

            return result;
        }


        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var employees = context.Employees.Include(x => x.Department)
                .Where(x => x.Department.Name == "Research and Development")
                .OrderBy(x => x.Salary)
                .ThenByDescending(x => x.FirstName)
                .Select(x =>
                new
                {
                    x.FirstName,
                    x.LastName,
                    DepartmentName = x.Department.Name,
                    x.Salary
                })
                .ToList();

            StringBuilder result = new StringBuilder();

            foreach (var employee in employees)
            {
                result.AppendLine($"{employee.FirstName} {employee.LastName} from {employee.DepartmentName} - ${employee.Salary:F2}");
            }

            return result.ToString().TrimEnd();
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(x => x.Salary > 50000)
                .OrderBy(x => x.FirstName)
                .Select(x =>
                new
                {
                    x.FirstName,
                    x.Salary
                })
                .ToList();

            StringBuilder result = new StringBuilder();

            foreach (var employee in employees)
            {
                result.AppendLine($"{employee.FirstName} - {employee.Salary:F2}");
            }

            return result.ToString().TrimEnd();
        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {

            var employees = context.Employees
                .OrderBy(x => x.EmployeeId)
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    x.MiddleName,
                    x.JobTitle,
                    x.Salary,
                }).ToList();


            StringBuilder result = new StringBuilder();

            foreach (var employee in employees)
            {
                result.AppendLine($"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:F2}");
            }

            return result.ToString().TrimEnd();
        }

    }
}
