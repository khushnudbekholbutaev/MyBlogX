using BlogPostify.Domain.Commons;
using BlogPostify.Domain.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogPostify.Domain.Entities;

public class Post : Auditable<int>
{
    [Column(TypeName = "jsonb")] // PostgreSQL uchun
    public Dictionary<string, TranslationModel> Translations { get; set; } = new();
    public string CoverImage { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public bool IsPublished { get; set; }

    // Navigation Properties
    public ICollection<Comment> Comments { get; set; }
    public ICollection<Like> Likes { get; set; }
    public ICollection<BookMark> Bookmarks { get; set; }
    public ICollection<PostCategory> PostCategories { get; set; }
    public ICollection<PostTag> PostTags { get; set; }
}
