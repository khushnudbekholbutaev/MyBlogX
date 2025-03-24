using AutoMapper;
using BlogPostify.Data.IRepositories;
using BlogPostify.Domain.Configurations;
using BlogPostify.Domain.Entities;
using BlogPostify.Service.Commons.CollectionExtensions;
using BlogPostify.Service.Commons.Helpers;
using BlogPostify.Service.DTOs.Posts;
using BlogPostify.Service.Exceptions;
using BlogPostify.Service.Interfaces.Posts;
using BlogPostify.Service.Interfaces.Users;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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
        var imageFileName = Guid.NewGuid().ToString("N") + Path.GetExtension(dto.CoverImage.FileName);
        var imageRootPath = Path.Combine(WebEnvironmentHost.WebRootPath, "Media", "Posts", "Images", imageFileName);

        using (var stream = new FileStream(imageRootPath, FileMode.Create))
        {
            await dto.CoverImage.CopyToAsync(stream);
        }

        var imageResult = Path.Combine("Media", "Posts", "Images", imageFileName);
        #endregion

        var mapped = mapper.Map<Post>(dto);
        mapped.CreatedAt = DateTime.UtcNow;
        mapped.CoverImage = imageResult;
        mapped.Translations = dto.Translations ?? new Dictionary<string, TranslationModel>(); // ✅ Null emasligiga ishonch hosil qilish

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
    public async Task<IEnumerable<PostForResultDto>> RetrieveAllAsync(PaginationParams @params)
    {
        var posts = await repository.SelectAll()
                                    .Include(p => p.Bookmarks)
                                    .Include(p => p.Comments)
                                    .Include(p => p.Likes)
                                    .Include(p => p.PostCategories)
                                    .Include(p => p.PostTags)
                                    .ToPagedList(@params)
                                    .AsNoTracking()
                                    .ToListAsync();

        return posts.Select(post =>
        {
            var dto = mapper.Map<PostForResultDto>(post);
            dto.Translations = post.Translations; // Deserialize qilish shart emas!
            return dto;
        }).ToList();
    }

    public async Task<PostForResultDto> RetrieveIdAsync(int id)
    {
        var post = await repository.SelectAll()
                                   .Where(p => p.Id == id)
                                   .AsNoTracking()
                                   .FirstOrDefaultAsync()
        ?? throw new BlogPostifyException(404, "Post is not found");

        var result = mapper.Map<PostForResultDto>(post);
        result.Translations = post.Translations; // JSON deserialize qilish shart emas!

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
        post.Translations = dto.Translations ?? new Dictionary<string, TranslationModel>(); // ✅ Null emasligiga ishonch hosil qilish
        post.UpdatedAt = DateTime.UtcNow;

        await repository.UpdateAsync(post);
        return mapper.Map<PostForResultDto>(post);
    }

}
