using AutoMapper;
using BlogPostify.Data.IRepositories;
using BlogPostify.Domain.Configurations;
using BlogPostify.Domain.Entities;
using BlogPostify.Service.Commons.CollectionExtensions;
using BlogPostify.Service.DTOs.PostTags;
using BlogPostify.Service.Exceptions;
using BlogPostify.Service.Interfaces.Posts;
using BlogPostify.Service.Interfaces.PostTags;
using BlogPostify.Service.Interfaces.Tags;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;

namespace BlogPostify.Service.Services.PostTags;

public class PostTagService : IPostTagService
{
    private readonly IMapper mapper;
    private readonly ITagService tagService;
    private readonly IPostService postService;
    private readonly IRepository<PostTag,int> repository;

    public PostTagService(
        IMapper mapper,
        ITagService tagService,
        IPostService postService,
        IRepository<PostTag, int> repository)
    {
        this.mapper = mapper;
        this.tagService = tagService;
        this.postService = postService;
        this.repository = repository;
    }

    public async Task<PostTagForResultDto> AddAsync(PostTagForCreationDto dto)
    {
        var post = await postService.RetrieveIdAsync(dto.PostId)
            ??throw new BlogPostifyException(404, "Post is not found");
        var tag = await tagService.RetrieveByIdAsync(dto.TagId)
            ??throw new BlogPostifyException(404, "Tag is not found");

        var mapped = mapper.Map<PostTag>(dto);
        mapped.CreatedAt = DateTime.UtcNow;
        await repository.InsertAsync(mapped);

        return mapper.Map<PostTagForResultDto>(mapped);
    }

    public async Task<PostTagForResultDto> ModifyAsync(int id, PostTagForUpdateDto dto)
    {
        var postTag = await repository.SelectAll().Where(p => p.Id == id).FirstOrDefaultAsync()
            ?? throw new BlogPostifyException(404, "Post Tag is not found");
        var mapped = mapper.Map(dto, postTag);
        mapped.UpdatedAt = DateTime.UtcNow;
        await repository.UpdateAsync(mapped);

        return mapper.Map<PostTagForResultDto>(mapped);
    }

    public async Task<bool> RemoveAsync(int id)
    {
        var postTag = await repository.SelectAll().Where(p => p.Id == id).FirstOrDefaultAsync()
            ?? throw new BlogPostifyException(404, "Post Tag is not found");

        await repository.DeleteAsync(id);
        return true;
    }

    public async Task<IEnumerable<PostTagForResultDto>> RetrieveAllAsync(PaginationParams @params)
    {
        var postTags = await repository.SelectAll().ToPagedList(@params).ToListAsync();
        return mapper.Map<IEnumerable<PostTagForResultDto>>(postTags);
    }

    public async Task<PostTagForResultDto> RetrieveByIdsync(int id)
    {
        var postTag = await repository.SelectAll().Where(p => p.Id == id).FirstOrDefaultAsync()
            ?? throw new BlogPostifyException(404, "Post Tag is not found");

        return mapper.Map<PostTagForResultDto>(postTag);
    }
}
