using BlogPostify.Domain.Configurations;
using BlogPostify.Domain.Enums;
using BlogPostify.Service.DTOs.UserRoles;
using BlogPostify.Service.Interfaces.Users;
using Microsoft.AspNetCore.Mvc;
using ResultWrapper.Library;

namespace BlogPostify.Api.Controllers.Users;

public class UserRoleController : BaseController
{
    private readonly IUserRoleService userRoleService;

    public UserRoleController(IUserRoleService userRoleService)
    {
        this.userRoleService = userRoleService;
    }
    [HttpPost]
    public async Task<Wrapper> InsertAsync([FromForm] UserRoleForCreationDto dto)
    {
        var result = await userRoleService.AddUserRoleAsync(dto);
        return new Wrapper(result);
    }

    [HttpGet]
    public async Task<Wrapper> GetAllAsync([FromQuery] PaginationParams @params)
    {
        var result = await userRoleService.GetAllUserRolesAsync(@params);
        return new Wrapper(result);
    }

    [HttpGet("{id}")]
    public async Task<Wrapper> GetByRoleNameAsync(Role role)
    {
        var result = await userRoleService.GetUserRoleByRoleNameAsync(role);
        return new Wrapper(result);
    }

    [HttpDelete("{id}")]
    public async Task<Wrapper> DeleteAsync([FromRoute] int id)
    {
        var result = await userRoleService.DeleteUserRoleAsync(id);
        return new Wrapper(result);
    }

    [HttpPut("{id}")]
    public async Task<Wrapper> UpdateAsync([FromRoute] int id, [FromBody] UserRoleForUpdateDto dto)
    {
        var result = await userRoleService.UpdateUserRoleAsync(id, dto);
        return new Wrapper(result);
    }
}
