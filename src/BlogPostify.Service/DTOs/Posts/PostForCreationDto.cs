using BlogPostify.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BlogPostify.Service.DTOs.Posts;

public class PostForCreationDto
{
    public int UserId { get; set; }
    public IFormFile CoverImage { get; set; }
    public bool IsPublished { get; set; }

    // Barcha tillar uchun tarjimalar JSON sifatida
    [Required]
    public Dictionary<string, TranslationModel> Translations { get; set; }
}
