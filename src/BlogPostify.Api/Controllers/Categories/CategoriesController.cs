using BlogPostify.Domain.Configurations;
using BlogPostify.Service.DTOs.Categories;
using BlogPostify.Service.Interfaces.Categories;
using Microsoft.AspNetCore.Mvc;
using ResultWrapper.Library;

namespace BlogPostify.Api.Controllers.Categories;

public class CategoriesController : BaseController
{
    private readonly ICategoryService categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        this.categoryService = categoryService;
    }
    [HttpPost]
    public async Task<Wrapper> InsertAsync([FromBody] CategoryForCreationDto dto)
    {
        var result = await categoryService.AddAsync(dto);
        return new Wrapper(result);
    }

    [HttpGet]
    public async Task<Wrapper> GetAllAsync([FromQuery] PaginationParams @params)
    {
        var result = await categoryService.RetrieveAllAsync(@params);
        return new Wrapper(result);
    }

    [HttpGet("{id}")]
    public async Task<Wrapper> GetByIdAsync([FromRoute] int id)
    {
        var result = await categoryService.RetrieveByIdAsync(id);
        return new Wrapper(result);
    }

    [HttpGet("{language}")]
    public async Task<Wrapper> GetAsync(string language)
    {
        var result = await categoryService.RetrieveByLanguageAsync(language);
        return new Wrapper(result);
    }

    [HttpDelete("{id}")]
    public async Task<Wrapper> DeleteAsync([FromRoute] int id)
    {
        var result = await categoryService.RemoveAsync(id);
        return new Wrapper(result);
    }

    [HttpPut("{id}")]
    public async Task<Wrapper> UpdateAsync([FromRoute] int id, [FromBody] CategoryForUpdateDto dto)
    {
        var result = await categoryService.ModifyAsync(id, dto);
        return new Wrapper(result);
    }
}
