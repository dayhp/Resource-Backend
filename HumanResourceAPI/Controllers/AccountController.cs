using AutoMapper;
using Entities.Dto;
using Entities.Models;
using HumanResourceAPI.Infrastrcuture;
using HumanResourceAPI.Infrastrcuture.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HumanResourceAPI.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private static ILoggerManager _logger;
        private static IRepositoryManager _repository;
        private static IMapper _mapper;
        private static UserManager<User> _userManager;
        private static IAuthenticationManager _authenticationManager;
        public AccountController(
            ILoggerManager logger,
            IRepositoryManager repository,
            IMapper mapper,
            UserManager<User> userManager,
            IAuthenticationManager authenticationManager)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _userManager = userManager;
            _authenticationManager = authenticationManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserForRegistertrationDto request)
        {
            try
            {
                if (request == null)
                {
                    _logger.LogError("UserForRegistertrationDto object sent from client is null.");
                    return BadRequest("UserForRegistertrationDto object is null");
                }
                var user = new User
                {
                    UserName = request.UserName,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    PhoneNumber = request.PhoneNumber
                };

                //var user = _mapper.Map<User>(request);
                var result = await _userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest(ModelState);
                }
                if (request.Roles != null && request.Roles.Any())
                {
                    var roleResult = await _userManager.AddToRolesAsync(user, request.Roles);
                    if (!roleResult.Succeeded)
                    {
                        foreach (var error in roleResult.Errors)
                        {
                            ModelState.AddModelError(error.Code, error.Description);
                        }
                        return BadRequest(ModelState);
                    }
                }
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error at: {nameof(CreateUser)} with message {ex.Message}");
                return StatusCode(500, $"Internal server error");
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> AuthenticateUser([FromBody] UserForAuthenticationDto user)
        {
            if (await _authenticationManager.ValidateUserAsync(user))
            {
                return Ok(new { Token = await _authenticationManager.CreateTokenAsync() });
            }
            _logger.LogWarn($"Authentication failed. Wrong user name or password.");
            return Unauthorized();
        }
    }
}
