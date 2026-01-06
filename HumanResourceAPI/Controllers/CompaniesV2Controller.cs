using AutoMapper;
using HumanResourceAPI.Infrastrcuture;
using HumanResourceAPI.Infrastrcuture.Repository;
using Microsoft.AspNetCore.Mvc;

namespace HumanResourceAPI.Controllers
{
    [ApiVersion("2.0")]
    [ApiController]
    [Route("api/companies")]
    [ApiExplorerSettings(GroupName = "v2")]
    public class CompaniesV2Controller : ControllerBase
    {
        private static ILoggerManager _logger;
        private static IRepositoryManager _repository;
        private static IMapper _mapper;
        public CompaniesV2Controller(
            ILoggerManager logger,
            IRepositoryManager repository,
            IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCompanies()
        {
            var companies = await _repository.Company.GetCompaniesAsync(trackChanges: false);
            return Ok(companies);
        }
    }
}
