using BlogPostify.Domain.Configurations;
using BlogPostify.Service.DTOs.Comments;
using BlogPostify.Service.DTOs.Tags;
using BlogPostify.Service.Interfaces.Comments;
using BlogPostify.Service.Services.Tags;
using Microsoft.AspNetCore.Mvc;
using ResultWrapper.Library;

namespace BlogPostify.Api.Controllers.Comments;

public class CommentsController : BaseController
{
    private readonly ICommentService commentService;

    public CommentsController(ICommentService commentService)
    {
        this.commentService = commentService;
    }
    [HttpPost]
    public async Task<Wrapper> InsertAsync([FromBody] CommentForCreationDto dto)
    {
        var result = await commentService.AddAsync(dto);
        return new Wrapper(result);
    }

    [HttpGet]
    public async Task<Wrapper> GetAllAsync([FromQuery] int postId, [FromQuery]PaginationParams @params)
    {
        var result = await commentService.RetrieveAllAsync(postId, @params);
        return new Wrapper(result);
    }

    [HttpGet("{id}")]
    public async Task<Wrapper> GetByIdAsync([FromRoute] int id)
    {
        var result = await commentService.RetrieveByIdAsync(id);
        return new Wrapper(result);
    }

    [HttpDelete("{id}")]
    public async Task<Wrapper> DeleteAsync([FromRoute] int id)
    {
        var result = await commentService.RemoveAsync(id);
        return new Wrapper(result);
    }

    [HttpPut("{id}")]
    public async Task<Wrapper> UpdateAsync([FromRoute] int id, [FromBody] CommentForUpdateDto dto)
    {
        var result = await commentService.ModifyAsync(id, dto);
        return new Wrapper(result);
    }
}
