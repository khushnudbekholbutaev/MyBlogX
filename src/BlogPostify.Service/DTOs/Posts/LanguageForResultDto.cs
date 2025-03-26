using BlogPostify.Domain.Commons;

namespace BlogPostify.Service.DTOs.Posts;

public class LanguageForResultDto
{
    public string Title { get; set; }
    public string Content { get; set; }
    public string CoverImage { get; set; } = "string";
    public int UserId { get; set; }
    public bool IsPublished { get; set; }
}
