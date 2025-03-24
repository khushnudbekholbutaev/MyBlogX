namespace BlogPostify.Service.DTOs.Comments;

public class CommentForCreationDto
{
    public int PostId { get; set; }
    public int UserId { get; set; }
    public string Content { get; set; }
    public long? ParentCommentId { get; set; }
}
