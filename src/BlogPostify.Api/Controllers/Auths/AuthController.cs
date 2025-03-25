using BlogPostify.Service.DTOs.Auths;
using BlogPostify.Service.Interfaces.Auths;
using Microsoft.AspNetCore.Mvc;

namespace BlogPostify.Api.Controllers.Auths;

public class AuthController : BaseController
{
    private readonly IAuthService authService;

    public AuthController(IAuthService authService)
    {
        this.authService = authService;
    }
    [HttpPost("authenticate")]

    public async Task<IActionResult> PostAsync(LoginDto dto)
    {
        var token = await authService.AuthenticateAsync(dto);
        return Ok(token);
    }
}
