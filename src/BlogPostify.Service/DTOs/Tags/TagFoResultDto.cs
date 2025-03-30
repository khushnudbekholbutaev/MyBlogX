using BlogPostify.Domain.Entities;

namespace BlogPostify.Service.DTOs.Tags;

public class TagFoResultDto
{
    public long Id { get; set; }
    public string TagName { get; set; }

    // Relations
    //public ICollection<PostTag> PostTags { get; set; }
}
