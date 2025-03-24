using BlogPostify.Domain.Commons;

namespace BlogPostify.Domain.Entities;

public class PostCategory: Auditable<int>
{
    public int PostId { get; set; }
    public Post Post { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
}
