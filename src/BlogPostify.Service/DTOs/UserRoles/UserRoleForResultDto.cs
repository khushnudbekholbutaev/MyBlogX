using BlogPostify.Domain.Enums;

namespace BlogPostify.Service.DTOs.UserRoles;

public class UserRoleForResultDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public Role Role { get; set; }
}
