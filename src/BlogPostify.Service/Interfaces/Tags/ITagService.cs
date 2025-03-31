using BlogPostify.Domain.Configurations;
using BlogPostify.Service.DTOs.Posts;
using BlogPostify.Service.DTOs.Tags;

namespace BlogPostify.Service.Interfaces.Tags;

public interface ITagService
{
    Task<bool> RemoveAsync(long id);
    Task<TagFoResultDto> RetrieveByIdAsync(long id);
    Task<IEnumerable<TagFoResultDto>> RetrieveAllAsync();
    Task<TagFoResultDto> AddAsync(TagForCreationDto dto);
    Task<TagFoResultDto> ModifyAsync(long id,TagForUpdateDto dto);
}
