using BlogPostify.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace BlogPostify.Service.DTOs.Posts;

public class PostForUpdateDto
{
    public int UserId { get; set; }
    public IFormFile CoverImage { get; set; }
    public bool IsPublished { get; set; }

    // Yangi tarjimalarni saqlash
    public Dictionary<string, TranslationModel> Translations { get; set; }
}
