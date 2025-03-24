using AutoMapper;
using BlogPostify.Data.IRepositories;
using BlogPostify.Domain.Configurations;
using BlogPostify.Domain.Entities;
using BlogPostify.Service.Commons.CollectionExtensions;
using BlogPostify.Service.DTOs.Likes;
using BlogPostify.Service.Exceptions;
using BlogPostify.Service.Interfaces.Likes;
using BlogPostify.Service.Interfaces.Posts;
using BlogPostify.Service.Interfaces.Users;
using Microsoft.EntityFrameworkCore;

namespace BlogPostify.Service.Services.Likes;

public class LikeService : ILikeService
{
    private readonly IMapper mapper;
    private readonly IUserService userService;
    private readonly IPostService postService;
    private readonly IRepository<Like,long> repository;

    public LikeService(
        IMapper mapper,
        IUserService userService,
        IPostService postService,
        IRepository<Like, long> repository)
    {
        this.mapper = mapper;
        this.repository = repository;
        this.userService = userService;
        this.postService = postService;
    }

    public async Task<LikeForResultDto> AddAsync(LikeForCreationDto dto)
    {
        var user = await userService.RetrieveByIdasync(dto.UserId)
            ?? throw new BlogPostifyException(404, "User is not found");
        var post = await postService.RetrieveIdAsync(dto.PostId)
            ?? throw new BlogPostifyException(404, "Post is not found");
        var mapped = mapper.Map<Like>(dto);
        mapped.CreatedAt = DateTime.UtcNow;
        await repository.InsertAsync(mapped);

        return mapper.Map<LikeForResultDto>(mapped);
    }

    public async Task<LikeForResultDto> ModifyAsync(long id, LikeForUpdateDto dto)
    {
        var like = await repository.SelectAll().Where(x => x.Id == id).FirstOrDefaultAsync()
            ?? throw new BlogPostifyException(404, "Like is not found");
        var mapped = mapper.Map(dto, like);
        mapped.UpdatedAt = DateTime.UtcNow;

        await repository.UpdateAsync(mapped);
        return mapper.Map<LikeForResultDto>(mapped);
    }

    public async Task<bool> RemoveAsync(long id)
    {
        var like = await repository.SelectAll().Where(l => l.Id == id).FirstOrDefaultAsync()
            ?? throw new BlogPostifyException(404, "Like is not found");
        await repository.DeleteAsync(id);
        return true;
    }

    public async Task<IEnumerable<LikeForResultDto>> RetrieveAllAsync(PaginationParams @params)
    {
        var likes = await repository.SelectAll()
                                    .ToPagedList(@params)
                                    .AsQueryable()
                                    .ToListAsync();

        return mapper.Map<IEnumerable<LikeForResultDto>>(likes);
    }

    public async Task<LikeForResultDto> RetrieveByIdAsync(long id)
    {
        var like = await repository.SelectAll().Where(l => l.Id == id).FirstOrDefaultAsync()
            ?? throw new BlogPostifyException(404, "Like is not found");
        return mapper.Map<LikeForResultDto>(like);
    }
}
