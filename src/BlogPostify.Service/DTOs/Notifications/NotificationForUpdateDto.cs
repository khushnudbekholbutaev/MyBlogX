namespace BlogPostify.Service.DTOs.Notifications;

public class NotificationForUpdateDto
{
    public int UserId { get; set; }
    public string Message { get; set; }
    public bool IsRead { get; set; }
}
