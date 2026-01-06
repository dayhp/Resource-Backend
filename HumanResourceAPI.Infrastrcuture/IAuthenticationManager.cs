using Entities.Dto;

namespace HumanResourceAPI.Infrastrcuture
{
    public interface IAuthenticationManager
    {
        Task<bool> ValidateUserAsync(UserForAuthenticationDto userForAuth);
        Task<string> CreateTokenAsync();
    }
}
