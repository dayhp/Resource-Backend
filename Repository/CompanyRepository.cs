using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using HumanResourceAPI.Infrastrcuture.Repository;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class CompanyRepository : RepositoryBase<Company, Guid>, ICompanyRepository
    {
        public CompanyRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<PageList<Company>> GetAllCompaniesAsync(CompanyParameters companyParameters, bool trackChanges)
        {
            var companies = await FindAll(trackChanges)
                .OrderBy(c => c.Name)
                .ToListAsync();
            return PageList<Company>.ToPageList(companies, companyParameters.PageNumber, companyParameters.PageSize);
        }

        public async Task<IEnumerable<Company>> GetCompaniesAsync(bool trackChanges)
        {
            return await FindAll(trackChanges)
                .OrderBy(n => n.Name)
                .ToListAsync();
        }
    }
}
