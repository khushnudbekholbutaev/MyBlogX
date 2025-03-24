using BlogPostify.Domain.Commons;
using BlogPostify.Domain.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogPostify.Domain.Entities;


public class BookMark : Auditable<int>
{
    public int UserId { get; set; }
    public User User { get; set; }

    public int PostId { get; set; }
    public Post Post { get; set; }
}
