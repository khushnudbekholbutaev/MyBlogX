
using BlogPostify.Service.DTOs.Tags;
using BlogPostify.Service.Interfaces.Tags;
using BlogPostify.Service.Services.Posts;
using Microsoft.AspNetCore.Mvc;
using ResultWrapper.Library;

namespace BlogPostify.Api.Controllers.Tags;

public class TagsController : BaseController
{
    private readonly ITagService tagService;

    public TagsController(ITagService tagService)
    {
        this.tagService = tagService;
    }
    [HttpPost]
    public async Task<Wrapper> InsertAsync([FromBody] TagForCreationDto dto)
    {
        var result = await tagService.AddAsync(dto);
        return new Wrapper(result);
    }

    [HttpGet]
    public async Task<Wrapper> GetAllAsync()
    {
        var result = await tagService.RetrieveAllAsync();
        return new Wrapper(result);
    }

    [HttpGet("{id}")]
    public async Task<Wrapper> GetByIdAsync([FromRoute] int id)
    {
        var result = await tagService.RetrieveByIdAsync(id);
        return new Wrapper(result);
    }

    [HttpDelete("{id}")]
    public async Task<Wrapper> DeleteAsync([FromRoute] int id)
    {
        var result = await tagService.RemoveAsync(id);
        return new Wrapper(result);
    }

    [HttpPut("{id}")]
    public async Task<Wrapper> UpdateAsync([FromRoute] int id, [FromBody] TagForUpdateDto dto)
    {
        var result = await tagService.ModifyAsync(id, dto);
        return new Wrapper(result);
    }
}
