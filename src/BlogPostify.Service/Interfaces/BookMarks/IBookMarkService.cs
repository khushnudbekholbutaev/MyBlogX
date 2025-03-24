using BlogPostify.Domain.Configurations;
using BlogPostify.Service.DTOs.BookMarks;

namespace BlogPostify.Service.Interfaces.BookMarks;

public interface IBookMarkService
{
    Task<bool> RemoveAsync(int id);
    Task<BookMarkForResultDto> RetrieveByIdAsync(int id);
    Task<BookMarkForResultDto> AddAsync(BookMarkForCreationDto dto);
    Task<BookMarkForResultDto> ModifyAsync(long id,BookMarkForUpdateDto dto);
    Task<IEnumerable<BookMarkForResultDto>> RetrieveAllAsync(PaginationParams @params);
}
