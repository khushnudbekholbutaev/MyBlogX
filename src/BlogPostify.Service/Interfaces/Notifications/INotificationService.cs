using BlogPostify.Domain.Configurations;
using BlogPostify.Service.DTOs.Notifications;

namespace BlogPostify.Service.Interfaces.Notifications;

public interface INotificationService
{
    Task<bool> RemoveAsync(long id);
    Task<NotificationForResultDto> RetrieveByIdAsync(long id);
    Task<NotificationForResultDto> AddAsync(NotificationForCreationDto dto);
    Task<NotificationForResultDto> ModifyAsync(long id,NotificationForUpdateDto dto);
    Task<IEnumerable<NotificationForResultDto>> RetrieveAllAsync(PaginationParams @params);
}
