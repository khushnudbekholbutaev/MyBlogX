using BlogPostify.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace BlogPostify.Service.DTOs.Users;

public class UserForCreationDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Bio { get; set; }
    public IFormFile ProfileImageUrl { get; set; }
}
