using AutoMapper;
using BlogPostify.Data.IRepositories;
using BlogPostify.Domain.Configurations;
using BlogPostify.Domain.Entities;
using BlogPostify.Service.Commons.CollectionExtensions;
using BlogPostify.Service.DTOs.Notifications;
using BlogPostify.Service.Exceptions;
using BlogPostify.Service.Interfaces.Notifications;
using BlogPostify.Service.Interfaces.Users;
using Microsoft.EntityFrameworkCore;

namespace BlogPostify.Service.Services.Notifications;

public class NotificationService : INotificationService
{
    private readonly IMapper mapper;
    private readonly IUserService userService;
    private readonly IRepository<Notification,long> repository;

    public NotificationService(
        IMapper mapper,
        IUserService userService,
        IRepository<Notification, long> repository)
    {
        this.mapper = mapper;
        this.repository = repository;
        this.userService = userService;
    }


    public async Task<NotificationForResultDto> AddAsync(NotificationForCreationDto dto)
    {
        var user = await userService.RetrieveByIdasync(dto.UserId)
            ?? throw new BlogPostifyException(404, "USer is not found");
        var mapped = mapper.Map<Notification>(dto);
        mapped.CreatedAt = DateTime.UtcNow;
        await repository.InsertAsync(mapped);

        return mapper.Map<NotificationForResultDto>(mapped);
    }

    public async Task<NotificationForResultDto> ModifyAsync(long id, NotificationForUpdateDto dto)
    {
        var notification = await repository.SelectAll()
            .Where(n => n.Id == id)
            .FirstOrDefaultAsync()
            ?? throw new BlogPostifyException(404, "Notification is not found");
        var mapped = mapper.Map(dto, notification);
        mapped.UpdatedAt = DateTime.UtcNow;
        await repository.UpdateAsync(mapped);

        return mapper.Map<NotificationForResultDto>(mapped);
    }

    public async Task<bool> RemoveAsync(long id)
    {
        if (!await repository.SelectAll().AnyAsync(n => n.Id == id))
            throw new BlogPostifyException(404, "Notification is not found");
        await repository.DeleteAsync(id);
        return true;
    }

    public async Task<IEnumerable<NotificationForResultDto>> RetrieveAllAsync(PaginationParams @params)
    {
        var notifications = await repository.SelectAll()
            .ToPagedList(@params)
            .AsQueryable()
            .ToListAsync();
        return mapper.Map<IEnumerable<NotificationForResultDto>>(notifications);
    }

    public async Task<NotificationForResultDto> RetrieveByIdAsync(long id)
    {
        var notificatoin = await repository.SelectAll()
            .Where(n => n.Id == id)
            .FirstOrDefaultAsync()
            ?? throw new BlogPostifyException(404, "Notfication is not found");

        return mapper.Map<NotificationForResultDto>(notificatoin);
    }
}
