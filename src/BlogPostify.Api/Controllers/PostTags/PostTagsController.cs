using BlogPostify.Domain.Configurations;
using BlogPostify.Service.DTOs.PostTags;
using BlogPostify.Service.Interfaces.PostTags;
using Microsoft.AspNetCore.Mvc;
using ResultWrapper.Library;

namespace BlogPostify.Api.Controllers.PostTags;

public class PostTagsController : BaseController
{
    private readonly IPostTagService postTagService;

    public PostTagsController(IPostTagService postTagService)
    {
        this.postTagService = postTagService;
    }
    [HttpPost]
    public async Task<Wrapper> InsertAsync([FromBody] PostTagForCreationDto dto)
    {
        var result = await postTagService.AddAsync(dto);
        return new Wrapper(result);
    }

    [HttpGet]
    public async Task<Wrapper> GetAllAsync([FromQuery] PaginationParams @params)
    {
        var result = await postTagService.RetrieveAllAsync(@params);
        return new Wrapper(result);
    }

    [HttpGet("{id}")]
    public async Task<Wrapper> GetByIdAsync([FromRoute] int id)
    {
        var result = await postTagService.RetrieveByIdsync(id);
        return new Wrapper(result);
    }

    [HttpDelete("{id}")]
    public async Task<Wrapper> DeleteAsync([FromRoute] int id)
    {
        var result = await postTagService.RemoveAsync(id);
        return new Wrapper(result);
    }

    [HttpPut("{id}")]
    public async Task<Wrapper> UpdateAsync([FromRoute] int id, [FromBody] PostTagForUpdateDto dto)
    {
        var result = await postTagService.ModifyAsync(id, dto);
        return new Wrapper(result);
    }
}
