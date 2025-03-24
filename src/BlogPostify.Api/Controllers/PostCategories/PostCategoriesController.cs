using BlogPostify.Domain.Configurations;
using BlogPostify.Service.DTOs.PostCategories;
using BlogPostify.Service.Interfaces.PostCategories;
using Microsoft.AspNetCore.Mvc;
using ResultWrapper.Library;

namespace BlogPostify.Api.Controllers.PostCategories;

public class PostCategoriesController : BaseController
{
    private readonly IPostCategoryService postCategoryService;

    public PostCategoriesController(IPostCategoryService postCategoryService)
    {
        this.postCategoryService = postCategoryService;
    }
    [HttpPost]
    public async Task<Wrapper> InsertAsync([FromBody] PostCategoryForCreationDto dto)
    {
        var result = await postCategoryService.AddAsync(dto);
        return new Wrapper(result);
    }

    [HttpGet]
    public async Task<Wrapper> GetAllAsync([FromQuery] PaginationParams @params)
    {
        var result = await postCategoryService.RetrieveAllAsync(@params);
        return new Wrapper(result);
    }

    [HttpGet("{id}")]
    public async Task<Wrapper> GetByIdAsync([FromRoute] int id)
    {
        var result = await postCategoryService.RetrieveByIdAsync(id);
        return new Wrapper(result);
    }

    [HttpDelete("{id}")]
    public async Task<Wrapper> DeleteAsync([FromRoute] int id)
    {
        var result = await postCategoryService.RemoveAsync(id);
        return new Wrapper(result);
    }

    [HttpPut("{id}")]
    public async Task<Wrapper> UpdateAsync([FromRoute] int id, [FromBody] PostCategoryForUpdateDto dto)
    {
        var result = await postCategoryService.ModifyAsync(id, dto);
        return new Wrapper(result);
    }

}
