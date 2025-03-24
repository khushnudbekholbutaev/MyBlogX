using BlogPostify.Domain.Entities;

namespace BlogPostify.Service.DTOs.Posts;

public class PostForResultDto
{
    public int Id { get; set; }
    public string CoverImage { get; set; }
    public int UserId { get; set; }
    public bool IsPublished { get; set; }
}
