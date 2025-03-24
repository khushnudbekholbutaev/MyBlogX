using BlogPostify.Domain.Entities;

namespace BlogPostify.Service.DTOs.Posts;

public class PostForResultDto
{
    public int Id { get; set; }
    public string CoverImage { get; set; }
    public int UserId { get; set; }
    public bool IsPublished { get; set; }

    // Barcha tarjimalarni JSON'dan parse qilish uchun
    public Dictionary<string, TranslationModel> Translations { get; set; }
}
