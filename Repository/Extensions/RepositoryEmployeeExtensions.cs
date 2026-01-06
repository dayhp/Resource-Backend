using Entities.Models;
using Entities.RequestFeatures;
using System.Linq.Dynamic.Core;
using System.Text;

namespace Repository.Extensions
{
    public static class RepositoryEmployeeExtensions
    {
        public static IQueryable<Employee> FilterEmployees(
            this IQueryable<Employee> employees,
            EmployeeParameters employeeParameters) =>
            employees.Where(e => e.Age >= employeeParameters.MinAge && e.Age <= employeeParameters.MaxAge);

        public static IQueryable<Employee> Search(
            this IQueryable<Employee> employees,
            string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return employees;
            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return employees.Where(e => e.FirstName.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<Employee> Sort(
            this IQueryable<Employee> employees,
            string? orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return employees.OrderBy(e => e.FirstName);
            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(Employee).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            var orderQuerryBuilder = new StringBuilder();
            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;
                var propertyFromQueryName = param.Split(" ")[0];
                var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));
                if (objectProperty == null)
                    continue;
                var direction = param.EndsWith(" desc") ? "descending" : "ascending";
                orderQuerryBuilder.Append($"{objectProperty.Name} {direction}, ");
            }
            var orderQuery = orderQuerryBuilder.ToString().TrimEnd(',', ' ');
            if (string.IsNullOrWhiteSpace(orderQuery))
            {
                return employees.OrderBy(e => e.FirstName);
            }
            // Use System.Linq.Dynamic.Core to support dynamic OrderBy
            return employees.OrderBy(orderQuery);
        }
    }
}
