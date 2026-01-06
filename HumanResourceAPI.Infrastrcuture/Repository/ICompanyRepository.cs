using Entities.Models;
using Entities.RequestFeatures;

namespace HumanResourceAPI.Infrastrcuture.Repository
{
    public interface ICompanyRepository : IRepositoryBase<Company, Guid>
    {
        Task<IEnumerable<Company>> GetCompaniesAsync(bool trackChanges);
        Task<PageList<Company>> GetAllCompaniesAsync(CompanyParameters companyParameters, bool trackChanges);
    }
}