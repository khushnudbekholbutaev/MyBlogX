using BlogPostify.Domain.Configurations;
using BlogPostify.Service.DTOs.Users;

namespace BlogPostify.Service.Interfaces.Users;

public interface IUserService
{
    Task<bool> RemoveAsync(int id);
    Task<UserForResultDto> RetrieveByIdasync(int id);
    Task<UserForResultDto> AddAsync(UserForCreationDto dto);
    Task<UserForResultDto> ModifyAsync(int id, UserForUpdateDto dto);
    Task<IEnumerable<UserForResultDto>> RetrieveAllAsync(PaginationParams @params);
}
