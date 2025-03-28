using BlogPostify.Service.DTOs.Auths;

namespace BlogPostify.Service.Interfaces.Auths;

public interface IRefreshTokenService
{
    Task<LoginResultDto> RefreshTokenAsync(RefreshTokenDto dto);
}
