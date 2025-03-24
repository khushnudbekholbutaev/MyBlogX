namespace BlogPostify.Service.DTOs.Notifications;

public class NotificationForResultDto
{
    public long Id { get; set; }
    public int UserId { get; set; }
    public string Message { get; set; }
    public bool IsRead { get; set; }
}
