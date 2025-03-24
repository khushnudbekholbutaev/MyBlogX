using BlogPostify.Domain.Commons;
using BlogPostify.Domain.Entities.Users;

namespace BlogPostify.Domain.Entities;

public class Notification : Auditable<long>
{
    public int UserId { get; set; }
    public User User { get; set; }
    public string Message { get; set; }
    public bool IsRead { get; set; }
}
