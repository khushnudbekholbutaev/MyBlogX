using BlogPostify.Service.DTOs.Auths;

namespace BlogPostify.Service.Interfaces.Auths;

public interface IAuthService
{
    Task<LoginResultDto> AuthenticateAsync(LoginDto dto);
}
