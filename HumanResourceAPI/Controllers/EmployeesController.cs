using AutoMapper;
using Entities.Dto;
using Entities.Models;
using Entities.RequestFeatures;
using HumanResourceAPI.Infrastrcuture;
using HumanResourceAPI.Infrastrcuture.Repository;
using Microsoft.AspNetCore.Mvc;

namespace HumanResourceAPI.Controllers
{
    [ApiController]
    [Route("api/companies/{companyId}/employees")]
    public class EmployeesController : Controller
    {
        private static ILoggerManager _logger;
        private static IRepositoryManager _repository;
        private static IMapper _mapper;
        public EmployeesController(
            ILoggerManager logger,
            IRepositoryManager repository,
            IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeesForCompany(Guid companyId, [FromQuery] EmployeeParameters employeeParameters)
        {
            if (!employeeParameters.ValidAgeRange)
            {
                return BadRequest("Max age can't be less than min age.");
            }
            var company = await _repository.Company.FindByIdAsync(companyId);
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} not found.");
                return NotFound();
            }
            var employeesFromDb = await _repository.Employee.GetEmployeesAsync(companyId, employeeParameters, trackChanges: false);
            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb);

            var response = new PagedResponse<EmployeeDto>(employeesDto, employeesFromDb.Pagination);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(Guid companyId, [FromBody] EmployeeForCreationDto employee)
        {
            var company = await _repository.Company.FindByIdAsync(companyId);
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} not found.");
                return NotFound();
            }
            var employeeEntity = _mapper.Map<Employee>(employee);
            employeeEntity.CompanyId = companyId;
            _repository.Employee.Create(employeeEntity);
            await _repository.SaveAsync();
            var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);
            return CreatedAtRoute("GetEmployeeForCompany", new { companyId, id = employeeToReturn.Id }, employeeToReturn);
        }

        [HttpGet("{id}", Name = "GetEmployeeForCompany")]
        public async Task<IActionResult> GetEmployeesForCompany(Guid companyId, Guid id)
        {
            var company = _repository.Company.FindById(companyId);
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} not found.");
                return NotFound();
            }
            var employeesFromDb = _repository.Employee.FindByIdAsync(id);
            if (employeesFromDb == null)
            {
                _logger.LogInfo($"Employee with id: {id} not found.");
                return NotFound();
            }
            var employeesDto = _mapper.Map<EmployeeDto>(employeesFromDb);
            return Ok(employeesDto);
        }

        [HttpPost("collection")]
        public async Task<IActionResult> CreateEmployeeCollection(Guid companyId, [FromBody] IEnumerable<EmployeeForCreationDto> employeeCollection)
        {
            var company = await _repository.Company.FindByIdAsync(companyId);
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} not found.");
                return NotFound();
            }
            var employeeEntities = _mapper.Map<IEnumerable<Employee>>(employeeCollection);
            foreach (var employee in employeeEntities)
            {
                employee.CompanyId = companyId;
                _repository.Employee.Create(employee);
            }
            await _repository.SaveAsync();
            var employeesToReturn = _mapper.Map<IEnumerable<EmployeeDto>>(employeeEntities);
            return Ok(employeesToReturn);
        }
    }
}
