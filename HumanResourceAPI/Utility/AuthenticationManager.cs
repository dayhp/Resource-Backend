using Entities.Dto;
using Entities.Models;
using HumanResourceAPI.Infrastrcuture;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HumanResourceAPI.Utility
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private User _user;
        public AuthenticationManager(
            UserManager<User> userManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        public async Task<string> CreateTokenAsync()
        {
            var signingCredentials = SigningCredentials();
            var clams = await GetClams();
            var tokenOptions = GenerateTokenOptions(signingCredentials, clams);
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        public async Task<bool> ValidateUserAsync(UserForAuthenticationDto userForAuth)
        {
            var user = await _userManager.FindByNameAsync(userForAuth.UserName);
            if (user == null)
            {
                return false;
            }
            var isValidPassWord = await _userManager.CheckPasswordAsync(user, userForAuth.Password);
            if (user == null || isValidPassWord == false)
            {
                return false;
            }
            else
            {
                _user = user;
                return true;
            }
        }
        private async Task<List<Claim>> GetClams()
        {
            var clams = new List<Claim>
            {
                new Claim(
                    ClaimTypes.Email,
                    ClaimTypes.Name,
                    _user.UserName,
                    ClaimTypes.Expiration
                    )
            };
            var roles = await _userManager.GetRolesAsync(_user);
            foreach (var role in roles)
            {
                clams.Add(new Claim(ClaimTypes.Role, role));
            }
            return clams;
        }
        private SigningCredentials SigningCredentials()
        {
            var key = _configuration["JwtSettings:secretKey"];
            var secret = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(key));
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private JwtSecurityToken GenerateTokenOptions(
            SigningCredentials signingCredentials,
            List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var tokenOptions = new JwtSecurityToken(
                issuer: jwtSettings.GetSection("validIssuer").Value,
                audience: jwtSettings.GetSection("validAudience").Value,
                claims: claims,
                expires: DateTime.Now.AddMinutes(
                    Convert.ToDouble(jwtSettings.GetSection("expires").Value)),
                signingCredentials: signingCredentials);
            return tokenOptions;
        }
    }
}
