using BlogPostify.Domain.Commons;
using BlogPostify.Domain.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;
using BlogPostify.Domain.Commons;
namespace BlogPostify.Domain.Entities;

public class Post : Auditable<int>
{
    public MultyLanguageField Title { get; set; }
    public MultyLanguageField Content { get; set; }
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
