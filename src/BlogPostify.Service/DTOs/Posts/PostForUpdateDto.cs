using BlogPostify.Domain.Commons;
using BlogPostify.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace BlogPostify.Service.DTOs.Posts;

public class PostForUpdateDto
{
    public int UserId { get; set; }
    public bool IsPublished { get; set; }
    public IFormFile CoverImage { get; set; }
    public MultyLanguageField Title { get; set; }
    public MultyLanguageField Content { get; set; }
}
