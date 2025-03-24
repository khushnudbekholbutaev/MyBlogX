using BlogPostify.Api.Helpers;
using BlogPostify.Domain.Configurations;
using BlogPostify.Service.DTOs.Users;
using BlogPostify.Service.Interfaces.Users;
using Microsoft.AspNetCore.Mvc;
using ResultWrapper.Library;

namespace BlogPostify.Api.Controllers.Users
{
    public class UsersController : BaseController
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        public async Task<Wrapper> InsertAsync([FromForm] UserForCreationDto dto)
        {
            var result = await userService.AddAsync(dto);
            return new Wrapper(result);
        }

        [HttpGet]
        public async Task<Wrapper> GetAllAsync([FromQuery] PaginationParams @params)
        {
            var result = await userService.RetrieveAllAsync(@params);
            return new Wrapper(result);
        }

        [HttpGet("{id}")]
        public async Task<Wrapper> GetByIdAsync([FromRoute] int id)
        {
            var result = await userService.RetrieveByIdasync(id);
            return new Wrapper(result);
        }

        [HttpDelete("{id}")]
        public async Task<Wrapper> DeleteAsync([FromRoute] int id)
        {
            var result = await userService.RemoveAsync(id);
            return new Wrapper(result);
        }

        [HttpPut("{id}")]
        public async Task<Wrapper> UpdateAsync([FromRoute] int id, [FromBody] UserForUpdateDto dto)
        {
            var result = await userService.ModifyAsync(id, dto);
            return new Wrapper(result);
        }
    }
}
