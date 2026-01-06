
using Entities;
using HumanResourceAPI.Infrastrcuture.Repository;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly AppDbContext _dbContext;
        private ICompanyRepository _companyRepository;
        private IEmployeeRepository _empRepository;
        public RepositoryManager(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public ICompanyRepository Company
        {
            get
            {
                if (_companyRepository == null)
                {
                    _companyRepository = new CompanyRepository(_dbContext);
                }
                return _companyRepository;
            }
        }

        public IEmployeeRepository Employee
        {
            get
            {
                if (_empRepository == null)
                {
                    _empRepository = new EmployeeRepository(_dbContext);
                }
                return _empRepository;
            }
        }

        public async Task SaveAsync() => await _dbContext.SaveChangesAsync();
    }
}
