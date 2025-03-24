using AutoMapper;
using BlogPostify.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using BlogPostify.Service.Exceptions;
using BlogPostify.Data.IRepositories;
using BlogPostify.Domain.Configurations;
using BlogPostify.Service.Interfaces.Posts;
using BlogPostify.Service.DTOs.PostCategories;
using BlogPostify.Service.Interfaces.Categories;
using BlogPostify.Service.Interfaces.PostCategories;
using BlogPostify.Service.Commons.CollectionExtensions;

namespace BlogPostify.Service.Services.PostCategories;

public class PostCategoryService : IPostCategoryService
{
    private readonly IMapper mapper;
    private readonly IPostService postService;
    private readonly ICategoryService categoryService;
    private readonly IRepository<PostCategory,int> repository;

    public PostCategoryService(
        IMapper mapper, 
        IPostService postService,
        ICategoryService categoryService,
        IRepository<PostCategory, int> repository)
    {
        this.mapper = mapper;
        this.repository = repository;
        this.postService = postService;
        this.categoryService = categoryService;
    }

    public async Task<PostCategoryForResultDto> AddAsync(PostCategoryForCreationDto dto)
    {
        if (await categoryService.RetrieveByIdAsync(dto.CategoryId) == null)
            throw new BlogPostifyException(404, "Category is not found");

        if (await postService.RetrieveIdAsync(dto.PostId) == null)
            throw new BlogPostifyException(404, "Post is not found");

        var mapped = mapper.Map<PostCategory>(dto);
        mapped.CreatedAt = DateTime.UtcNow;
        await repository.InsertAsync(mapped);

        return mapper.Map<PostCategoryForResultDto>(mapped);
    }



    public async Task<PostCategoryForResultDto> ModifyAsync(int id, PostCategoryForUpdateDto dto)
    {
        var postCategory = await repository.SelectAll()
                                                .Where(p => p.Id == id)
                                                .FirstOrDefaultAsync()
                                                ?? throw new BlogPostifyException(404, "Post Category is not found");
        var mapped = mapper.Map(dto, postCategory);
        mapped.UpdatedAt = DateTime.UtcNow;
        await repository.UpdateAsync(mapped);

        return mapper.Map<PostCategoryForResultDto>(mapped);
    }

    public async Task<bool> RemoveAsync(int id)
    {
        if (!await repository.SelectAll().AnyAsync(p => p.Id == id))
            throw new BlogPostifyException(404, "Post Tag is not found");
        await repository.DeleteAsync(id);

        return true;
    }

    public async Task<IEnumerable<PostCategoryForResultDto>> RetrieveAllAsync(PaginationParams @params)
    {
        var postTags = await repository.SelectAll()
                                                    .ToPagedList(@params)
                                                    .AsNoTracking()
                                                    .ToListAsync();

        return mapper.Map<IEnumerable<PostCategoryForResultDto>>(postTags);
    }

    public async Task<PostCategoryForResultDto> RetrieveByIdAsync(int id)
    {
        var postTag = await repository.SelectAll()
                                                .Where(p => p.Id == id)
                                                .AsNoTracking()
                                                .FirstOrDefaultAsync()
                                                ?? throw new BlogPostifyException(404, "Post Tag is not found");
        return mapper.Map<PostCategoryForResultDto>(postTag);
    }
}
