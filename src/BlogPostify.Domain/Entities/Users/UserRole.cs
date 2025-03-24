using BlogPostify.Domain.Commons;
using BlogPostify.Domain.Enums;

namespace BlogPostify.Domain.Entities.Users;

public class UserRole : Auditable<int>
{
    public int UserId { get; set; }
    public User User { get; set; }
    public Role Role { get; set; }  
}
