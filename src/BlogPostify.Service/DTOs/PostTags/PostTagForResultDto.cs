namespace BlogPostify.Service.DTOs.PostTags;

public class PostTagForResultDto
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public long TagId { get; set; }
}
