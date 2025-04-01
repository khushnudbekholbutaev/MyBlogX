using AutoMapper;
using Azure;
using Azure.Core;
using BlogPostify.Data.IRepositories;
using BlogPostify.Domain.Commons;
using BlogPostify.Domain.Configurations;
using BlogPostify.Domain.Entities;
using BlogPostify.Service.Commons.CollectionExtensions;
using BlogPostify.Service.Commons.Helpers;
using BlogPostify.Service.DTOs.PostCategories;
using BlogPostify.Service.DTOs.Posts;
using BlogPostify.Service.DTOs.PostTags;
using BlogPostify.Service.DTOs.Tags;
using BlogPostify.Service.Exceptions;
using BlogPostify.Service.Interfaces.Categories;
using BlogPostify.Service.Interfaces.PostCategories;
using BlogPostify.Service.Interfaces.Posts;
using BlogPostify.Service.Interfaces.PostTags;
using BlogPostify.Service.Interfaces.Tags;
using BlogPostify.Service.Interfaces.Users;
using BlogPostify.Service.Services.PostTags;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace BlogPostify.Service.Services.Posts;

public class PostService(
    IServiceProvider serviceProvider,
    IMapper mapper,
    IRepository<Post, int> repository,
    IRepository<Tag, long> tagrepository,
    IRepository<PostTag, int> ptrepository) : IPostService
{
    private readonly IMapper mapper = mapper;
    private readonly IRepository<Post,int> repository = repository;
    private readonly IRepository<Tag,long> tagrepository = tagrepository;
    private readonly IRepository<PostTag,int> ptrepository = ptrepository;

    private readonly IServiceProvider serviceProvider = serviceProvider;

    public async Task<PostForResultDto> AddAsync(PostForCreationDto dto)
    {

        using var scope = serviceProvider.CreateScope();
        var categoryService = scope.ServiceProvider.GetRequiredService<ICategoryService>();
        var tagService = scope.ServiceProvider.GetRequiredService<ITagService>();
        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
        var pcService = scope.ServiceProvider.GetRequiredService<IPostCategoryService>();
        var ptService = scope.ServiceProvider.GetRequiredService<IPostTagService>();

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

        var categoryIdsNames = new List<int>();
        var name_id = new Dictionary<long, string>();

        foreach (var tagInput in dto.Tags)
        {
            if (int.TryParse(tagInput, out int tagId))
            {
                var existingTag = await tagService.RetrieveByIdAsync(tagId) 
                    ?? throw new BlogPostifyException(404, $"Tag with ID {tagId} not found");
                name_id[existingTag.Id] = existingTag.TagName;
            }
            else
            {
                var existingTag = await tagrepository.SelectAll().FirstOrDefaultAsync(t => t.TagName == tagInput);
                if (existingTag == null)
                {
                    var tagForCreationDto = new TagForCreationDto
                    {
                        TagName = tagInput
                    };
                    var added = await tagService.AddAsync(tagForCreationDto);
                    name_id[added.Id] = added.TagName;
                }
                else { name_id[existingTag.Id] = existingTag.TagName; }
            }
        }

        foreach (var CategoryIdInput in dto.CategoryIds)
        {
            var existingCategory = await categoryService.RetrieveByIdAsync(CategoryIdInput)
                    ?? throw new BlogPostifyException(404, $"Tag with ID {CategoryIdInput} not found");
            categoryIdsNames.Add(existingCategory.Id);
        }
        var resultDto = mapper.Map<PostForResultDto>(mapped);

        resultDto.TagNames = [.. name_id.Values];
        await repository.InsertAsync(mapped);

        foreach(var CategoryIdInput in dto.CategoryIds)
        {
            var postCategoryDto = new PostCategoryForCreationDto
            {
                PostId = mapped.Id,
                CategoryId = CategoryIdInput
            };
            await pcService.AddAsync(postCategoryDto);
        }

        foreach (var tagId in name_id.Keys)
        {
            var postTagDto = new PostTagForCreationDto
            {
                PostId = mapped.Id,
                TagId = tagId
            };
            await ptService.AddAsync(postTagDto);
        }
        return resultDto;

    }

    public async Task<bool> RemoveAsync(int id)
    {
        var post = await repository.SelectAll().FirstOrDefaultAsync(p => p.Id == id)
            ?? throw new BlogPostifyException(404, "Post is not found");

        await repository.DeleteAsync(id);
        return true;
    }
    
    public async Task<List<LanguageResultDto>> RetrieveByLanguageAsync(string language, string tag)
    {
    #region
    //var posts = await repository.SelectAll()
    //.Where(p => p.IsPublished)
    //.ToListAsync();

    //if (!posts.Any())
    //    throw new KeyNotFoundException($"No published posts found!");

    //var results = new List<LanguageResultDto>();

    //foreach (var post in posts)
    //{
    //    var tagIds = await ptrepository.SelectAll()
    //        .Where(pt => pt.PostId == post.Id)
    //        .Select(pt => pt.TagId)
    //        .ToListAsync();

    //    var tagNames = await tagrepository.SelectAll()
    //        .Where(tag => tagIds.Contains(tag.Id))
    //        .Select(tag => tag.TagName)
    //        .ToListAsync();

    //    string title = GetLocalizedText(post.Title, language);
    //    string content = GetLocalizedText(post.Content, language);

    //    results.Add(new LanguageResultDto
    //    {
    //        Id = post.Id,
    //        Title = title,
    //        Content = content,
    //        UserId = post.UserId,
    //        TagNames = tagNames,
    //        CreatedAt = post.CreatedAt,
    //        CoverImage = post.CoverImage,
    //        IsPublished = post.IsPublished
    //    });
    //}

    //return results;
    #endregion

    List<Post> posts;

    if (string.IsNullOrWhiteSpace(tag))
    {
        posts = await repository.SelectAll()
            .Where(p => p.IsPublished)
            .ToListAsync();
    }
    else
    {
        var tagIds = await tagrepository.SelectAll()
            .Where(t => EF.Functions.Like(t.TagName, $"%{tag}%")) 
            .Select(t => t.Id)
            .ToListAsync();

        var postIds = await ptrepository.SelectAll()
            .Where(pt => tagIds.Contains(pt.TagId))
            .Select(pt => pt.PostId)
            .Distinct() 
            .ToListAsync();

        posts = await repository.SelectAll()
            .Where(p => postIds.Contains(p.Id) && p.IsPublished) 
            .ToListAsync();
    }

    if (!posts.Any())
        throw new KeyNotFoundException($"No published posts found!");

    var results = new List<LanguageResultDto>();

    foreach (var post in posts)
    {
        var postTagIds = await ptrepository.SelectAll()
            .Where(pt => pt.PostId == post.Id)
            .Select(pt => pt.TagId)
            .ToListAsync();

        var tagNames = await tagrepository.SelectAll()
            .Where(tag => postTagIds.Contains(tag.Id))
            .Select(tag => tag.TagName)
            .ToListAsync();

        string title = GetLocalizedText(post.Title, language);
        string content = GetLocalizedText(post.Content, language);

        results.Add(new LanguageResultDto
        {
            Id = post.Id,
            Title = title,
            Content = content,
            UserId = post.UserId,
            TagNames = tagNames, 
            CreatedAt = post.CreatedAt,
            CoverImage = post.CoverImage,
            IsPublished = post.IsPublished
        });
    }

    return results;
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

    public async Task<List<PostTitleDto>> SearchByTagAsync(string tag, string language)
    {
        var tagIds = await tagrepository.SelectAll()
            .Where(t => EF.Functions.Like(t.TagName, $"%{tag}%"))
            .Select(t => t.Id)
            .ToListAsync();

        var postIds = await ptrepository.SelectAll()
            .Where(pt => tagIds.Contains(pt.TagId))
            .Select(pt => pt.PostId)
            .Distinct()
            .ToListAsync();

        var posts = await repository.SelectAll()
            .Where(p => postIds.Contains(p.Id))
            .ToListAsync();

        var results = posts.Select(post => new PostTitleDto
        {
            Id = post.Id,
            Title = GetLocalizedText(post.Title, language)
        }).ToList();

        return results;
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
