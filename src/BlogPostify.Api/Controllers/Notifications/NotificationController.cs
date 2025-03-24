#region Libraries
using BlogPostify.Domain.Configurations;
using BlogPostify.Service.DTOs.Notifications;
using BlogPostify.Service.Interfaces.Notifications;
using Microsoft.AspNetCore.Mvc;
using ResultWrapper.Library;
#endregion
namespace BlogPostify.Api.Controllers.Notifications;

public class Notifications : BaseController
{
    private readonly INotificationService notificationService;

    public Notifications(INotificationService notificationService)
    {
        this.notificationService = notificationService;
    }
    [HttpPost]
    public async Task<Wrapper> InsertAsync([FromForm] NotificationForCreationDto dto)
    {
        var result = await notificationService.AddAsync(dto);
        return new Wrapper(result);
    }

    [HttpGet]
    public async Task<Wrapper> GetAllAsync([FromQuery] PaginationParams @params)
    {
        var result = await notificationService.RetrieveAllAsync(@params);
        return new Wrapper(result);
    }

    [HttpGet("{id}")]
    public async Task<Wrapper> GetByIdAsync([FromRoute] int id)
    {
        var result = await notificationService.RetrieveByIdAsync(id);
        return new Wrapper(result);
    }

    [HttpDelete("{id}")]
    public async Task<Wrapper> DeleteAsync([FromRoute] int id)
    {
        var result = await notificationService.RemoveAsync(id);
        return new Wrapper(result);
    }

    [HttpPut("{id}")]
    public async Task<Wrapper> UpdateAsync([FromRoute] int id, [FromBody] NotificationForUpdateDto dto)
    {
        var result = await notificationService.ModifyAsync(id, dto);
        return new Wrapper(result);
    }
}
