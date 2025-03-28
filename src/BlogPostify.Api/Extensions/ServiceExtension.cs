using BlogPostify.Data.IRepositories;
using BlogPostify.Data.Repositories;
using BlogPostify.Domain.Entities;
using BlogPostify.Service.Interfaces.Auths;
using BlogPostify.Service.Interfaces.BookMarks;
using BlogPostify.Service.Interfaces.Categories;
using BlogPostify.Service.Interfaces.Comments;
using BlogPostify.Service.Interfaces.Likes;
using BlogPostify.Service.Interfaces.Notifications;
using BlogPostify.Service.Interfaces.PostCategories;
using BlogPostify.Service.Interfaces.Posts;
using BlogPostify.Service.Interfaces.PostTags;
using BlogPostify.Service.Interfaces.Tags;
using BlogPostify.Service.Interfaces.Users;
using BlogPostify.Service.Mappers;
using BlogPostify.Service.Services.Auths;
using BlogPostify.Service.Services.BookMarks;
using BlogPostify.Service.Services.Categories;
using BlogPostify.Service.Services.Comments;
using BlogPostify.Service.Services.Likes;
using BlogPostify.Service.Services.Notifications;
using BlogPostify.Service.Services.PostCategories;
using BlogPostify.Service.Services.Posts;
using BlogPostify.Service.Services.PostTags;
using BlogPostify.Service.Services.Tags;
using BlogPostify.Service.Services.Users;

namespace BlogPostify.Api.Extensions;
public static class ServiceExtension
{
    public static void AddCustomServices(this IServiceCollection services)
    {
        //Folder Name: Generic Reporitory
        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

        //Folder Name: User Service
        services.AddScoped<IUserService, UserService>();

        // Folder Name : Auth Service
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();

        // Folder Name : User Role Service
        services.AddScoped<IUserRoleService,UserRoleService>();

        // Folder Name : Post Service
        services.AddScoped<IPostService,PostService>();

        // Folder Name : Category Service
        services.AddScoped<ICategoryService,CategoryService>();

        // Folder Name : Tag Service
        services.AddScoped<ITagService,TagService>();

        // Folder Name : PostCategory Service
        services.AddScoped<IPostCategoryService,PostCategoryService>();
        
        // FOlder Name : PostTag Service
        services.AddScoped<IPostTagService,PostTagService>();

        // FOlder Name : Comment Service
        services.AddScoped<ICommentService,CommentService>();

        // Folder Name : Like Service
        services.AddScoped<ILikeService,LikeService>();

        // Folder Name : BookMark Service
        services.AddScoped<IBookMarkService,BookMarkService>();

        // Folder Name : Notification Service
        services.AddScoped<INotificationService,NotificationService>();

        // Mapping
        services.AddAutoMapper(typeof(MappingProfile));
    }
}