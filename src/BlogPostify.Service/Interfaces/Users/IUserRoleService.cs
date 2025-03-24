using BlogPostify.Domain.Configurations;
using BlogPostify.Domain.Enums;
using BlogPostify.Service.DTOs.UserRoles;

namespace BlogPostify.Service.Interfaces.Users;

public interface IUserRoleService
{
    Task<bool> DeleteUserRoleAsync(int id);
    Task<UserRoleForResultDto> AddUserRoleAsync(UserRoleForCreationDto userRole);
    Task<IEnumerable<UserRoleForResultDto>> GetUserRoleByRoleNameAsync(Role role);
    Task<IEnumerable<UserRoleForResultDto>> GetAllUserRolesAsync(PaginationParams @params);
    Task<UserRoleForResultDto> UpdateUserRoleAsync(int id, UserRoleForUpdateDto userRole);
}
