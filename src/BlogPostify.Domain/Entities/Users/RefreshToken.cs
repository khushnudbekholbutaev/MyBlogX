using BlogPostify.Domain.Commons;

namespace BlogPostify.Domain.Entities.Users;

public class RefreshToken : Auditable<int>
{
    public string Token { get; set; }
    public DateTime ExpiryDate { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}
