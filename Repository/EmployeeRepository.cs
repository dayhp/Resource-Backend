using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using HumanResourceAPI.Infrastrcuture.Repository;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;

namespace Repository
{
    public class EmployeeRepository : RepositoryBase<Employee, Guid>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<PageList<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges)
        {
            //var employees = await FindAll(trackChanges)
            //    .Where(e => e.CompanyId.Equals(companyId) &&
            //                e.Age >= employeeParameters.MinAge && e.Age <= employeeParameters.MaxAge)
            //    .OrderBy(e => e.FirstName)
            //    .ToListAsync();

            var employees = await FindAll(trackChanges)
                .Where(e => e.CompanyId.Equals(companyId))
                .FilterEmployees(employeeParameters)
                .Search(employeeParameters.SearchTerm)
                .Sort(employeeParameters.OrderBy)
                .ToListAsync();

            return PageList<Employee>.ToPageList(employees, employeeParameters.PageNumber, employeeParameters.PageSize);
        }
    }
}
