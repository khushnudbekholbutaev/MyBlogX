using AutoMapper;
using BlogPostify.Data.IRepositories;
using BlogPostify.Domain.Configurations;
using BlogPostify.Domain.Entities;
using BlogPostify.Service.Commons.CollectionExtensions;
using BlogPostify.Service.DTOs.BookMarks;
using BlogPostify.Service.Exceptions;
using BlogPostify.Service.Interfaces.BookMarks;
using BlogPostify.Service.Interfaces.Posts;
using BlogPostify.Service.Interfaces.Users;
using Microsoft.EntityFrameworkCore;

namespace BlogPostify.Service.Services.BookMarks;

public class BookMarkService : IBookMarkService
{
    private readonly IMapper mapper;
    private readonly IUserService userService;
    private readonly IPostService postService;
    private readonly IRepository<BookMark,int> repository;

    public BookMarkService(
        IMapper mapper,
        IUserService userService,
        IPostService postService,
        IRepository<BookMark, int> repository)
    {
        this.mapper = mapper;
        this.repository = repository;
        this.userService = userService;
        this.postService = postService;
    }

    public async Task<BookMarkForResultDto> AddAsync(BookMarkForCreationDto dto)
    {
        var user = await userService.RetrieveByIdasync(dto.UserId)
            ?? throw new BlogPostifyException(404, "User is not found");
        var post = await postService.RetrieveIdAsync(dto.PostId)
            ?? throw new BlogPostifyException(404, "Post is not found");
        var mapped = mapper.Map<BookMark>(dto);
        mapped.CreatedAt = DateTime.UtcNow;
        await repository.InsertAsync(mapped);

        return mapper.Map<BookMarkForResultDto>(mapped);
    }

    public async Task<BookMarkForResultDto> ModifyAsync(long id, BookMarkForUpdateDto dto)
    {
        var bookMark = await repository.SelectAll().Where(b => b.Id == id).FirstOrDefaultAsync()
            ?? throw new BlogPostifyException(404, "BookMark is not found");

        var mapped = mapper.Map(dto,bookMark);
        mapped.UpdatedAt = DateTime.UtcNow;
        await repository.UpdateAsync(mapped);

        return mapper.Map<BookMarkForResultDto>(mapped);
    }

    public async Task<bool> RemoveAsync(int id)
    {
        if (!await repository.SelectAll().AnyAsync(b => id == id))
            throw new BlogPostifyException(404, "BookMark is not found");
        await repository.DeleteAsync(id);   
        return true;
    }

    public async Task<IEnumerable<BookMarkForResultDto>> RetrieveAllAsync(PaginationParams @params)
    {
        var bookMarks = await repository.SelectAll().ToPagedList(@params).AsQueryable().ToListAsync();

        return mapper.Map<IEnumerable<BookMarkForResultDto>>(bookMarks);
    }

    public async Task<BookMarkForResultDto> RetrieveByIdAsync(int id)
    {
        var bookMark = await repository.SelectAll().Where(b => b.Id == id).FirstOrDefaultAsync()
            ?? throw new BlogPostifyException(404, "BookMark is not found");
        
        return mapper.Map<BookMarkForResultDto>(bookMark);
    }
}
