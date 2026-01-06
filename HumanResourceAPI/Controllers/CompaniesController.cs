using AutoMapper;
using Entities.Dto;
using Entities.Models;
using Entities.RequestFeatures;
using HumanResourceAPI.Infrastrcuture;
using HumanResourceAPI.Infrastrcuture.Repository;
using HumanResourceAPI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HumanResourceAPI.Controllers
{
    [ApiVersion("1.0", Deprecated = true)]
    [ApiController]
    [Route("api/companies")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class CompaniesController : ControllerBase
    {
        private static ILoggerManager _logger;
        private static IRepositoryManager _repository;
        private static IMapper _mapper;
        private readonly CompanyLinks _companyLinks;
        private static class RouteNames
        {
            public const string GetCompanies = nameof(GetCompanies);
            public const string GetCompany = nameof(GetCompany);
            public const string CreateCompany = nameof(CreateCompany);
            public const string DeleteCompany = nameof(DeleteCompany);
            public const string UpdateCompany = nameof(UpdateCompany);
            public const string PartiallyUpdateCompany = nameof(PartiallyUpdateCompany);
        }
        public CompaniesController(
            ILoggerManager logger,
            IRepositoryManager repository,
            IMapper mapper,
            CompanyLinks companyLinks)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _companyLinks = companyLinks;
        }
        [HttpGet(Name = RouteNames.GetCompanies)]
        [Authorize]
        public async Task<IActionResult> GetAllCompany([FromQuery] CompanyParameters companyParameters)
        {
            try
            {
                var companies = await _repository.Company.GetAllCompaniesAsync(companyParameters, trackChanges: false);
                var companyDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
                //return PageRespone(companyDto, companies.Pagination, includeHeader: true);
                //var response = new PagedResponse<CompanyDto>(companyDto, companies.Pagination);
                //return Ok(response);

                var links = _companyLinks.TryGenerateLinks(companyDto, companyParameters.Fields, HttpContext);
                return links.HasLinks ? Ok(links.LinkedEntities) : Ok(links.ShapedEntities);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error at: {nameof(GetAllCompany)} with message {ex.Message}");
                return StatusCode(500, $"Internal server error");
            }
        }

        [HttpGet("{id}", Name = RouteNames.GetCompany)]
        public IActionResult GetCompany(Guid id)
        {
            try
            {
                var company = _repository.Company.FindById(id);
                if (company == null)
                {
                    _logger.LogInfo($"Company with id: {id} not found.");
                    return NotFound();
                }
                else
                {
                    var companyDto = _mapper.Map<CompanyDto>(company);
                    return Ok(companyDto);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error at: {nameof(GetCompany)} with message {ex.Message}");
                return StatusCode(500, $"Internal server error");
            }
        }
        [HttpPost(Name = RouteNames.CreateCompany)]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyCreationDto input)
        {
            try
            {
                if (input == null)
                {
                    _logger.LogError("Company object sent from client is null.");
                    return BadRequest("Company object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid company object sent from client.");
                    //return BadRequest("Invalid model object");
                    return UnprocessableEntity(ModelState);
                }
                var companyEntity = _mapper.Map<Company>(input);
                _repository.Company.Create(companyEntity);
                await _repository.SaveAsync();
                var createdCompany = _mapper.Map<CompanyDto>(companyEntity);
                return CreatedAtRoute(RouteNames.GetCompanies, new { id = createdCompany.Id }, createdCompany);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error at: {nameof(CreateCompany)} with message {ex.Message}");
                return StatusCode(500, $"Internal server error");
            }
        }

        [HttpDelete("{id}", Name = RouteNames.DeleteCompany)]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            try
            {
                var company = _repository.Company.FindById(id);
                if (company == null)
                {
                    _logger.LogInfo($"Company with id: {id} not found.");
                    return NotFound();
                }
                _repository.Company.Delete(company);
                await _repository.SaveAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error at: {nameof(DeleteCompany)} with message {ex.Message}");
                return StatusCode(500, $"Internal server error");
            }
        }

        [HttpPut("{id}", Name = RouteNames.UpdateCompany)]
        public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyUpdateDto input)
        {
            try
            {
                if (input == null)
                {
                    _logger.LogError("Company object sent from client is null.");
                    return BadRequest("Company object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid company object sent from client.");
                    return BadRequest("Invalid model object");
                }
                var companyEntity = _repository.Company.FindById(id);
                if (companyEntity == null)
                {
                    _logger.LogInfo($"Company with id: {id} not found.");
                    return NotFound();
                }
                _mapper.Map(input, companyEntity);
                _repository.Company.Update(companyEntity);
                await _repository.SaveAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error at: {nameof(UpdateCompany)} with message {ex.Message}");
                return StatusCode(500, $"Internal server error");
            }
        }

        [HttpPatch("{id}", Name = RouteNames.PartiallyUpdateCompany)]
        public async Task<IActionResult> PartiallyUpdateCompany(Guid id,
            [FromBody] Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<CompanyUpdateDto> patchDoc)
        {
            try
            {
                if (patchDoc == null)
                {
                    _logger.LogError("Patch document sent from client is null.");
                    return BadRequest("Patch document is null");
                }
                var companyEntity = _repository.Company.FindById(id);
                if (companyEntity == null)
                {
                    _logger.LogInfo($"Company with id: {id} not found.");
                    return NotFound();
                }
                var companyToPatch = _mapper.Map<CompanyUpdateDto>(companyEntity);
                TryValidateModel(companyToPatch);
                patchDoc.ApplyTo(companyToPatch, ModelState);
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid model state for the patch document.");
                    return BadRequest("Invalid model state");
                }
                _mapper.Map(companyToPatch, companyEntity);
                _repository.Company.Update(companyEntity);
                await _repository.SaveAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error at: {nameof(PartiallyUpdateCompany)} with message {ex.Message}");
                return StatusCode(500, $"Internal server error");
            }
        }
    }
}
