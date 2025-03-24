using BlogPostify.Domain.Configurations;
using BlogPostify.Service.DTOs.Posts;

namespace BlogPostify.Service.Interfaces.Posts;

public interface IPostService
{
    Task<bool> RemoveAsync(int id);
    Task<PostForResultDto> RetrieveIdAsync(int id);
    Task<PostForResultDto> AddAsync(PostForCreationDto dto);
    Task<PostForResultDto> ModifyAsync(int id,PostForUpdateDto dto);
    Task<IEnumerable<PostForResultDto>> RetrieveAllAsync(PaginationParams @params);
}
