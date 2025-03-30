using BlogPostify.Domain.Commons;
using BlogPostify.Domain.Entities;

namespace BlogPostify.Service.DTOs.Posts;

public class PostForResultDto
{
    public MultyLanguageField Title { get; set; }
    public MultyLanguageField Content { get; set; }
    public int Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string CoverImage { get; set; }
    public int UserId { get; set; }
    public List<string> TagNames { get; set; } = [];
    public bool IsPublished { get; set; }
}
