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

        // ParentCommentId = 0 bo‘lsa, uni null qilish (PostgreSQL bilan muammo bo‘lmasligi uchun)
        dto.ParentCommentId = (dto.ParentCommentId.HasValue && dto.ParentCommentId.Value == 0)
            ? null
            : dto.ParentCommentId;

        // Yangi comment yaratish
        var comment = mapper.Map<Comment>(dto);
        comment.CreatedAt = DateTime.UtcNow;

        // Agar ParentCommentId mavjud bo‘lsa, validatsiya qilish
        if (comment.ParentCommentId.HasValue)
        {
            var parentComment = await repository.SelectAll()
                .FirstOrDefaultAsync(c => c.Id == comment.ParentCommentId);

            if (parentComment == null)
                throw new BlogPostifyException(404, "Parent comment is not found");
        }

        // Commentni bazaga qo‘shish
        await repository.InsertAsync(comment);

        // Natijani qaytarish
        return mapper.Map<CommentForResultDto>(comment);
    }
    public async Task<CommentForResultDto> ModifyAsync(long id, CommentForUpdateDto dto)
    {
        // Commentni topish
        var comment = await repository.SelectAll()
            .Include(c => c.ParentComment) // Parent commentni olish
            .Include(c => c.Replies) // Replies ham bo‘lishi kerak 
            .FirstOrDefaultAsync(c => c.Id == id)
            ?? throw new BlogPostifyException(404, "Comment is not found");

        // DTO ma’lumotlarini mavjud commentga o‘tkazish
        mapper.Map(dto, comment);
        comment.UpdatedAt = DateTime.UtcNow;

        // ParentCommentId = 0 bo‘lsa, uni null qilish (PostgreSQL bilan muammo bo‘lmasligi uchun)
        dto.ParentCommentId = (dto.ParentCommentId.HasValue && dto.ParentCommentId.Value == 0)
            ? null
            : dto.ParentCommentId;

        // Agar ParentCommentId o‘zgargan bo‘lsa, validatsiya qilish
        if (dto.ParentCommentId != comment.ParentCommentId)
        {
            if (dto.ParentCommentId.HasValue) // Agar yangi ota comment ID bor bo‘lsa
            {
                var newParent = await repository.SelectAll()
                    .FirstOrDefaultAsync(c => c.Id == dto.ParentCommentId);

                if (newParent == null)
                    throw new BlogPostifyException(404, "Parent comment is not found");

                comment.ParentCommentId = newParent.Id; // Yangi parentni bog‘lash
            }
            else
            {
                comment.ParentCommentId = null; // Agar yangi ota comment kiritilmasa, null qilish
            }
        }

        // Commentni saqlash
        await repository.UpdateAsync(comment);

        // Natijani qaytarish
        return mapper.Map<CommentForResultDto>(comment);
    }

    public async Task<bool> RemoveAsync(long id)
    {
        if (!await repository.SelectAll().AnyAsync(c => c.Id == id))
             throw new BlogPostifyException(404, "Comment is not found.");

        await repository.DeleteAsync(id);
        return true;
    }

    public async Task<IEnumerable<CommentForResultDto>> RetrieveAllAsync(int postId, PaginationParams @params)
    {
        var commentsQuery = repository.SelectAll()
                                      .Where(c => c.PostId == postId)
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
