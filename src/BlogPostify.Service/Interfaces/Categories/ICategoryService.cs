using BlogPostify.Domain.Configurations;
using BlogPostify.Service.DTOs.Categories;

namespace BlogPostify.Service.Interfaces.Categories;

public interface ICategoryService
{
    Task<bool> RemoveAsync(int id);
    Task<CategoryForResultDto> RetrieveByIdAsync(int id);
    Task<CategoryForResultDto> AddAsync(CategoryForCreationDto dto);
    Task<CategoryForResultDto> ModifyAsync(int id,CategoryForUpdateDto dto);
    Task<IEnumerable<CategoryForResultDto>> RetrieveAllAsync(PaginationParams @params);
    Task<List<LanguageResultDto>> RetrieveByLanguageAsync(string language);
}
