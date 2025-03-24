using BlogPostify.Domain.Entities;

namespace BlogPostify.Service.DTOs.Comments;

public class CommentForResultDto
{
    public long Id { get; set; }
    public int PostId { get; set; }
    public int UserId { get; set; }
    public string Content { get; set; }
    public long ParentCommentId { get; set; }
    public ICollection<Comment> Replies { get; set; }
}
