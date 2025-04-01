using BlogPostify.Domain.Configurations;
using BlogPostify.Service.DTOs.Posts;   
using BlogPostify.Service.Interfaces.Posts;
using Microsoft.AspNetCore.Mvc;
using ResultWrapper.Library;

namespace BlogPostify.Api.Controllers.Posts;

public class PostsController : BaseController
{
    private readonly IPostService postService;

    public PostsController(IPostService postService)
    {
        this.postService = postService;
    }
    [HttpPost]
    public async Task<Wrapper> InsertAsync([FromForm] PostForCreationDto dto)
    {
        var result = await postService.AddAsync(dto);
        return new Wrapper(result);
    }

    [HttpGet("language")]
    public async Task<Wrapper> GetByLanguageAsync([FromQuery]string? tag)
    {
        var language = Request.Headers.ContainsKey("language") ? Request.Headers["language"].ToString() : "uz";

        var result = await postService.RetrieveByLanguageAsync(language, tag);
        return new Wrapper(result);
    }
    
    [HttpGet("tags")]
    public async Task<Wrapper> GetPostByTagAsync([FromQuery]string tag)
    {
        var language = Request.Headers.ContainsKey("language") ? Request.Headers["language"].ToString() : "uz";

        var result = await postService.SearchByTagAsync(tag, language);
        return new Wrapper(result);
    }

    [HttpGet("{id}")]
    public async Task<Wrapper> GetByIdAsync([FromRoute] int id)
    {
        var result = await postService.RetrieveIdAsync(id);
        return new Wrapper(result);
    }

    [HttpDelete("{id}")]
    public async Task<Wrapper> DeleteAsync([FromRoute] int id)
    {
        var result = await postService.RemoveAsync(id);
        return new Wrapper(result);
    }

    [HttpPut("{id}")]
    public async Task<Wrapper> UpdateAsync([FromRoute] int id, [FromForm] PostForUpdateDto dto)
    {
        var result = await postService.ModifyAsync(id, dto);
        return new Wrapper(result);
    }
}
