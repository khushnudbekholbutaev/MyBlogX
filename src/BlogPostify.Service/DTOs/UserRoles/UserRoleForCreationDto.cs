using BlogPostify.Domain.Enums;

namespace BlogPostify.Service.DTOs.UserRoles;

public class UserRoleForCreationDto
{
    public int UserId { get; set; }
    public Role Role { get; set; }
}
