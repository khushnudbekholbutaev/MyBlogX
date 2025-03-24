using BlogPostify.Domain.Configurations;
using BlogPostify.Service.DTOs.Likes;

namespace BlogPostify.Service.Interfaces.Likes;

public interface ILikeService
{
    Task<bool> RemoveAsync(long id);
    Task<LikeForResultDto> RetrieveByIdAsync(long id);
    Task<LikeForResultDto> AddAsync(LikeForCreationDto dto);
    Task<LikeForResultDto> ModifyAsync(long id,LikeForUpdateDto dto);
    Task<IEnumerable<LikeForResultDto>> RetrieveAllAsync(PaginationParams @params);
}
