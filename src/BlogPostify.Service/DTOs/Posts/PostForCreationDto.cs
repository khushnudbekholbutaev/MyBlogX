using BlogPostify.Domain.Commons;
using BlogPostify.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BlogPostify.Service.DTOs.Posts;

public class PostForCreationDto
{
    public MultyLanguageField Title { get; set; }
    public MultyLanguageField Content { get; set; }
    public int UserId { get; set; }
    public IFormFile CoverImage { get; set; }
    public bool IsPublished { get; set; }
    public List<string> Tags { get; set; }
    public List<int> CategoryIds { get; set; }
}
