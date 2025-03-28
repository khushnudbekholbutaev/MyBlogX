using AutoMapper;
using BlogPostify.Data.IRepositories;
using BlogPostify.Domain.Commons;
using BlogPostify.Domain.Configurations;
using BlogPostify.Domain.Entities;
using BlogPostify.Service.Commons.CollectionExtensions;
using BlogPostify.Service.Commons.Helpers;
using BlogPostify.Service.DTOs.Posts;
using BlogPostify.Service.Exceptions;
using BlogPostify.Service.Interfaces.Posts;
using BlogPostify.Service.Interfaces.Users;
using Microsoft.EntityFrameworkCore;

namespace BlogPostify.Service.Services.Posts;

public class PostService : IPostService
{
    private readonly IMapper mapper;
    private readonly IUserService userService;
    private readonly IRepository<Post,int> repository;

    public PostService(
        IMapper mapper, 
        IRepository<Post,int> repository,
        IUserService userService)
    {
        this.mapper = mapper;
        this.repository = repository;
        this.userService = userService;
    }

    public async Task<PostForResultDto> AddAsync(PostForCreationDto dto)
    {
        var user = await userService.RetrieveByIdasync(dto.UserId)
            ?? throw new BlogPostifyException(404, "User not found");

        #region Image
        //var imageFileName = Guid.NewGuid().ToString("N") + Path.GetExtension(dto.CoverImage.FileName);
        //var imageRootPath = Path.Combine(WebEnvironmentHost.WebRootPath, "Media", "Posts", "Images", imageFileName);

        //using (var stream = new FileStream(imageRootPath, FileMode.Create))
        //{
        //    await dto.CoverImage.CopyToAsync(stream);
        //}

        //var imageResult = Path.Combine("Media", "Posts", "Images", imageFileName);
        #endregion

        var mapped = mapper.Map<Post>(dto);
        mapped.CreatedAt = DateTime.UtcNow;
        mapped.CoverImage = "string";

        await repository.InsertAsync(mapped);
        return mapper.Map<PostForResultDto>(mapped);
    }

    public async Task<bool> RemoveAsync(int id)
    {
        var post = await repository.SelectAll().FirstOrDefaultAsync(p => p.Id == id)
            ?? throw new BlogPostifyException(404, "Post is not found");

        await repository.DeleteAsync(id);
        return true;
    }
    public async Task<LanguageResultDto> RetrieveByLanguageAsync(int id, string language)
    {
        var post = await repository.SelectAll()
            .Where(p => p.Id == id && p.IsPublished)
            .FirstOrDefaultAsync();

        if (post == null)
            throw new KeyNotFoundException($"Post with ID {id} not found!");

        if (post.Title == null || post.Content == null)
            throw new InvalidOperationException("Post Title or Content is null!");

        string title = GetLocalizedText(post.Title, language);
        string content = GetLocalizedText(post.Content, language);

        return new LanguageResultDto
        {
            Id = post.Id,
            Title = title,
            Content = content,
            CoverImage = post.CoverImage ?? "default-image.png",
            IsPublished = post.IsPublished
        };
    }

    private string GetLocalizedText(MultyLanguageField field, string language)
    {
        if (field == null)
            return "No translation available";

        return language switch
        {
            "uz" => !string.IsNullOrEmpty(field.Uz) ? field.Uz : field.Eng,
            "ru" => !string.IsNullOrEmpty(field.Ru) ? field.Ru : field.Eng,
            "eng" => !string.IsNullOrEmpty(field.Eng) ? field.Eng : field.Uz,
            "tr" => !string.IsNullOrEmpty(field.Tr) ? field.Tr : field.Eng,
            _ => field.Uz
        };
    }

    public async Task<PostForResultDto> RetrieveIdAsync(int id)
    {
        var post = await repository.SelectAll()
                                   .Where(p => p.Id == id)
                                   .AsNoTracking()
                                   .FirstOrDefaultAsync()
        ?? throw new BlogPostifyException(404, "Post is not found");

        var result = mapper.Map<PostForResultDto>(post);

        return result;
    }
    public async Task<PostForResultDto> ModifyAsync(int id, PostForUpdateDto dto)
    {
        var post = await repository.SelectAll()
                                   .Where(p => p.Id == id)
                                   .FirstOrDefaultAsync()
        ?? throw new BlogPostifyException(404, "Post is not found");

        #region Image
        if (dto.CoverImage != null)
        {
            var imageFullPath = Path.Combine(WebEnvironmentHost.WebRootPath, post.CoverImage);
            if (File.Exists(imageFullPath))
                File.Delete(imageFullPath);

            var imageFileName = Guid.NewGuid().ToString("N") + Path.GetExtension(dto.CoverImage.FileName);
            var imageRootPath = Path.Combine(WebEnvironmentHost.WebRootPath, "Media", "Posts", "Images", imageFileName);

            using (var stream = new FileStream(imageRootPath, FileMode.Create))
            {
                await dto.CoverImage.CopyToAsync(stream);
            }

            post.CoverImage = Path.Combine("Media", "Posts", "Images", imageFileName);
        }
        #endregion

        post.IsPublished = dto.IsPublished;
        post.UpdatedAt = DateTime.UtcNow;

        await repository.UpdateAsync(post);
        return mapper.Map<PostForResultDto>(post);
    }
}
