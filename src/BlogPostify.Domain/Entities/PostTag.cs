using BlogPostify.Domain.Commons;

namespace BlogPostify.Domain.Entities;

public class PostTag : Auditable<int>
{
    public long TagId { get; set; }
    public Tag Tag { get; set; }
    public int PostId { get; set; }
    public Post Post { get; set; }
}
