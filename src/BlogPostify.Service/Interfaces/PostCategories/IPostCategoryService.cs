using BlogPostify.Domain.Configurations;
using BlogPostify.Service.DTOs.PostCategories;

namespace BlogPostify.Service.Interfaces.PostCategories;

public interface IPostCategoryService
{
    Task<bool> RemoveAsync(int id);
    Task<PostCategoryForResultDto> RetrieveByIdAsync(int id);
    Task<PostCategoryForResultDto> AddAsync(PostCategoryForCreationDto dto);
    Task<PostCategoryForResultDto> ModifyAsync(int id,PostCategoryForUpdateDto dto);
    Task<IEnumerable<PostCategoryForResultDto>> RetrieveAllAsync(PaginationParams @params);
}
