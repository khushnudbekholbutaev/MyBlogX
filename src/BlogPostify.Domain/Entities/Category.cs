using BlogPostify.Domain.Commons;

namespace BlogPostify.Domain.Entities;

public class Category : Auditable<int>
{
    public MultyLanguageField Name { get; set; }
    public string? Description { get; set; }

    // Relations
    public ICollection<PostCategory> PostCategories { get; set; }
}
