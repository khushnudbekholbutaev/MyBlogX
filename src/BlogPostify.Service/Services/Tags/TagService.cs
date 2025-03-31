using AutoMapper;
using BlogPostify.Data.IRepositories;
using BlogPostify.Domain.Entities;
using BlogPostify.Service.DTOs.Posts;
using BlogPostify.Service.DTOs.Tags;
using BlogPostify.Service.Exceptions;
using BlogPostify.Service.Interfaces.Tags;
using Microsoft.EntityFrameworkCore;

namespace BlogPostify.Service.Services.Tags;

public class TagService : ITagService
{
    private readonly IMapper mapper;
    private readonly IRepository<Tag,long> repository;

    public TagService(IMapper mapper,
        IRepository<Tag, long> repository)
    {
        this.mapper = mapper;
        this.repository = repository;
    }

    public async Task<TagFoResultDto> AddAsync(TagForCreationDto dto)
    {
        //var check = await repository.SelectAll().FirstOrDefaultAsync(t => t.TagName == dto.TagName)
        //    ?? throw new BlogPostifyException(404, $"Tag with not found");
        var mapped = mapper.Map<Tag>(dto);
        mapped.CreatedAt = DateTime.UtcNow;
        await repository.InsertAsync(mapped);

        return mapper.Map<TagFoResultDto>(mapped);
    }

    public async Task<TagFoResultDto> ModifyAsync(long id, TagForUpdateDto dto)
    {
        var tag = await repository.SelectAll().FirstOrDefaultAsync(t => t.Id == id)
           ?? throw new BlogPostifyException(404, "Tag is not found");

        var mapped = mapper.Map(dto, tag);
        mapped.UpdatedAt = DateTime.UtcNow;
        await repository.UpdateAsync(mapped);

        return mapper.Map<TagFoResultDto>(mapped);
    }

    public async Task<bool> RemoveAsync(long id)
    {
        if (await repository.SelectAll().AnyAsync(t => t.Id == id))
            throw new BlogPostifyException(404, "Tag is not found");
        await repository.DeleteAsync(id);   
        return true;
    }

    public async Task<IEnumerable<TagFoResultDto>> RetrieveAllAsync()
    {
        var tags = await repository.SelectAll()
                                                .Include(t => t.PostTags)
                                                .AsNoTracking()
                                                .ToListAsync();

        return mapper.Map<IEnumerable<TagFoResultDto>>(tags);

    }

    public async Task<TagFoResultDto> RetrieveByIdAsync(long id)
    {
        var tag = await repository.SelectAll().Where(t => t.Id == id).FirstOrDefaultAsync()
            ?? throw new BlogPostifyException(404, "Tag is not found");

        return mapper.Map<TagFoResultDto>(tag);
    }
}
