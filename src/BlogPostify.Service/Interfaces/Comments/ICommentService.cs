using BlogPostify.Domain.Configurations;
using BlogPostify.Service.DTOs.Comments;

namespace BlogPostify.Service.Interfaces.Comments;

public interface ICommentService
{
    Task<bool> RemoveAsync(long id);
    Task<CommentForResultDto> RetrieveByIdAsync(long id);
    Task<CommentForResultDto> AddAsync(CommentForCreationDto dto);
    Task<CommentForResultDto> ModifyAsync(long id,CommentForUpdateDto dto);
    Task<IEnumerable<CommentForResultDto>> RetrieveAllAsync(int postId, PaginationParams @params);
}
