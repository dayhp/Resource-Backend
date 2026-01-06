using Entities.Models;
using Entities.RequestFeatures;

namespace HumanResourceAPI.Infrastrcuture.Repository
{
    public interface IEmployeeRepository : IRepositoryBase<Employee, Guid>
    {
        Task<PageList<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges);
    }
}