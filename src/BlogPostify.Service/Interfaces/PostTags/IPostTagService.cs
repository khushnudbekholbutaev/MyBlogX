using BlogPostify.Domain.Configurations;
using BlogPostify.Service.DTOs.PostTags;

namespace BlogPostify.Service.Interfaces.PostTags;

public interface IPostTagService
{
    Task<bool> RemoveAsync(int id);
    Task<PostTagForResultDto> RetrieveByIdsync(int id);
    Task<PostTagForResultDto> AddAsync(PostTagForCreationDto dto);
    Task<PostTagForResultDto> ModifyAsync(int id,PostTagForUpdateDto dto); 
    Task<IEnumerable<PostTagForResultDto>> RetrieveAllAsync(PaginationParams @params);
}
