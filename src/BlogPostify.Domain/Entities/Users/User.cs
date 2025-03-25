using BlogPostify.Domain.Commons;
using BlogPostify.Domain.Enums;

namespace BlogPostify.Domain.Entities.Users;

public class User : Auditable<int>
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Bio {  get; set; }
    public Role Role { get; set; }
    public string ProfileImageUrl { get; set; }

    // Navigation Properties
    public ICollection<Like> Likes { get; set; }
    public ICollection<Post> Posts { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public ICollection<BookMark> Bookmarks { get; set; }
    public ICollection<UserRole> UserRoles { get; set; }
    public ICollection<RefreshToken> RefreshTokens { get; set; }
    public ICollection<Notification> Notifications { get; set; }
}
