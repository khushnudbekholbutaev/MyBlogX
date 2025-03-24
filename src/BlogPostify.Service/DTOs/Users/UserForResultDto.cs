using BlogPostify.Domain.Entities;
using BlogPostify.Domain.Enums;

namespace BlogPostify.Service.DTOs.Users;

public class UserForResultDto
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Bio { get; set; }
    public string ProfileImageUrl { get; set; }

    public ICollection<Post> Posts { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public ICollection<BookMark> Bookmarks { get; set; }
}
