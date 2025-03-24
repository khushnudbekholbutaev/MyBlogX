using BlogPostify.Domain.Commons;
using BlogPostify.Domain.Entities.Users;

namespace BlogPostify.Domain.Entities;

public class Like : Auditable<long>
{
    public int PostId { get; set; }
    public Post Post { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }
}
