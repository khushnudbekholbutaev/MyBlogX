using AutoMapper;
using BlogPostify.Data.IRepositories;
using BlogPostify.Domain.Configurations;
using BlogPostify.Domain.Entities;
using BlogPostify.Service.Commons.CollectionExtensions;
using BlogPostify.Service.DTOs.Comments;
using BlogPostify.Service.Exceptions;
using BlogPostify.Service.Interfaces.Comments;
using BlogPostify.Service.Interfaces.Posts;
using BlogPostify.Service.Interfaces.Users;
using Microsoft.EntityFrameworkCore;

namespace BlogPostify.Service.Services.Comments;

public class CommentService : ICommentService
{
    private readonly IMapper mapper;
    private readonly IUserService userService;
    private readonly IPostService postService;
    private readonly IRepository<Comment,long> repository;

    public CommentService(
        IMapper mapper, 
        IUserService userService,
        IPostService postService,
        IRepository<Comment, long> repository)
    {
        this.mapper = mapper;
        this.repository = repository;
        this.userService = userService;
        this.postService = postService;
    }
    public async Task<CommentForResultDto> AddAsync(CommentForCreationDto dto)
    {
        // Foydalanuvchi va postni tekshirish
        var user = await userService.RetrieveByIdasync(dto.UserId)
            ?? throw new BlogPostifyException(404, "User is not found");
        var post = await postService.RetrieveIdAsync(dto.PostId)
            ?? throw new BlogPostifyException(404, "Post is not found");

        // Comment ni map qilish
        var mapped = mapper.Map<Comment>(dto);
        mapped.CreatedAt = DateTime.UtcNow;

        // Agar ParentCommentId null yoki 0 bo'lmasa, otasini topamiz
        if (dto.ParentCommentId.HasValue && dto.ParentCommentId.Value > 0)
        {
            var parentComment = await repository.SelectAll()
                                                .Include(c => c.Replies)
                                                .FirstOrDefaultAsync(c => c.Id == dto.ParentCommentId);

            if (parentComment == null)
                throw new BlogPostifyException(404, "Parent comment is not found");

            mapped.ParentComment = parentComment; // Ota sharhni bog'lash
            parentComment.Replies.Add(mapped); // Ota sharhga yangi javob qo'shish
        }
        else
        {
            mapped.ParentCommentId = null; // Agar ParentCommentId null yoki 0 bo'lsa, ParentCommentId ni null qilib qo'yish
        }

        // Comment ni bazaga qo'shish
        await repository.InsertAsync(mapped);

        // Natijani map qilish va qaytarish
        return mapper.Map<CommentForResultDto>(mapped);
    }
    public async Task<CommentForResultDto> ModifyAsync(long id, CommentForUpdateDto dto)
    {
        var comment = await repository.SelectAll()
            .Include(c => c.ParentComment) // Parent commentni olish
            .Include(c => c.Replies) // Replies ham bo‘lishi kerak 
            .FirstOrDefaultAsync(c => c.Id == id)
            ?? throw new BlogPostifyException(404, "Comment is not found");

        // DTO ma’lumotlarini mavjud commentga o‘tkazish
        mapper.Map(dto, comment);
        comment.UpdatedAt = DateTime.UtcNow;

        // **Agar commentning ota commenti o‘zgargan bo‘lsa**, uni yangilash
        if (dto.ParentCommentId != comment.ParentCommentId)
        {
            var newParent = await repository.SelectAll()
                                            .FirstOrDefaultAsync(c => c.Id == dto.ParentCommentId);

            if (dto.ParentCommentId > 0 && newParent == null)
                throw new BlogPostifyException(404, "Parent comment is not found");

            comment.ParentComment = newParent;
        }

        return mapper.Map<CommentForResultDto>(comment);
    }


    public async Task<bool> RemoveAsync(long id)
    {
        if (!await repository.SelectAll().AnyAsync(c => c.Id == id))
             throw new BlogPostifyException(404, "Comment is not found.");

        await repository.DeleteAsync(id);
        return true;
    }

    public async Task<IEnumerable<CommentForResultDto>> RetrieveAllAsync(PaginationParams @params)
    {
        var commentsQuery = repository.SelectAll()
                                      .Include(c => c.Replies);

        var comments = await Task.Run(() => commentsQuery.ToPagedList(@params).ToList()); 

        return comments.Select(comment => mapper.Map<CommentForResultDto>(comment));
    }


    public async Task<CommentForResultDto> RetrieveByIdAsync(long id)
    {
        var comment = await repository.SelectAll()
                                                 .Where(c => c.Id == id)
                                                 .AsNoTracking()
                                                 .FirstOrDefaultAsync()
                                                 ?? throw new BlogPostifyException(404, "Comment is not found");
        return mapper.Map<CommentForResultDto>(comment);
    }
}
