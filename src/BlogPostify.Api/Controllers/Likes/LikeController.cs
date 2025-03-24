using BlogPostify.Domain.Configurations;
using BlogPostify.Service.DTOs.Likes;
using BlogPostify.Service.DTOs.Posts;
using BlogPostify.Service.Interfaces.Likes;
using BlogPostify.Service.Services.Posts;
using Microsoft.AspNetCore.Mvc;
using ResultWrapper.Library;

namespace BlogPostify.Api.Controllers.Likes;

public class LikeController : BaseController
{
    private readonly ILikeService likeService;

    public LikeController(ILikeService likeService)
    {
        this.likeService = likeService;
    }
    [HttpPost]
    public async Task<Wrapper> InsertAsync([FromForm] LikeForCreationDto dto)
    {
        var result = await likeService.AddAsync(dto);
        return new Wrapper(result);
    }

    [HttpGet]
    public async Task<Wrapper> GetAllAsync([FromQuery] PaginationParams @params)
    {
        var result = await likeService.RetrieveAllAsync(@params);
        return new Wrapper(result);
    }

    [HttpGet("{id}")]
    public async Task<Wrapper> GetByIdAsync([FromRoute] int id)
    {
        var result = await likeService.RetrieveByIdAsync(id);
        return new Wrapper(result);
    }

    [HttpDelete("{id}")]
    public async Task<Wrapper> DeleteAsync([FromRoute] int id)
    {
        var result = await likeService.RemoveAsync(id);
        return new Wrapper(result);
    }

    [HttpPut("{id}")]
    public async Task<Wrapper> UpdateAsync([FromRoute] int id, [FromBody] LikeForUpdateDto dto)
    {
        var result = await likeService.ModifyAsync(id, dto);
        return new Wrapper(result);
    }
}
