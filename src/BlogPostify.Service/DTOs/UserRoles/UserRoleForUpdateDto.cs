using BlogPostify.Domain.Enums;

namespace BlogPostify.Service.DTOs.UserRoles;

public class UserRoleForUpdateDto
{
    public int UserId { get; set; }
    public Role Role { get; set; }
}
