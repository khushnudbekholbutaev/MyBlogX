using BlogPostify.Service.DTOs.Auths;
using BlogPostify.Service.Interfaces.Auths;
using Microsoft.AspNetCore.Mvc;

namespace BlogPostify.Api.Controllers.Auths;

public class RefreshTokenController : BaseController
{
    private readonly IRefreshTokenService refreshTokenService;

    public RefreshTokenController(IRefreshTokenService refreshTokenService)
    {
        this.refreshTokenService = refreshTokenService;
    }
    [HttpPost]
    public async Task<IActionResult> PostAsync(RefreshTokenDto dto)
    {
        var token = await this.refreshTokenService.RefreshTokenAsync(dto);
        return Ok(token);
    }
}
