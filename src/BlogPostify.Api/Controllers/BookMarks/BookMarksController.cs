using BlogPostify.Domain.Configurations;
using BlogPostify.Service.DTOs.BookMarks;
using BlogPostify.Service.Interfaces.BookMarks;
using Microsoft.AspNetCore.Mvc;
using ResultWrapper.Library;

namespace BlogPostify.Api.Controllers.BookMarks;

public class BookMarksController : BaseController
{
    private readonly IBookMarkService bookMarkService;

    public BookMarksController(IBookMarkService bookMarkService)
    {
        this.bookMarkService = bookMarkService;
    }
    [HttpPost]
    public async Task<Wrapper> InsertAsync([FromForm] BookMarkForCreationDto dto)
    {
        var result = await bookMarkService.AddAsync(dto);
        return new Wrapper(result);
    }

    [HttpGet]
    public async Task<Wrapper> GetAllAsync([FromQuery] PaginationParams @params)
    {
        var result = await bookMarkService.RetrieveAllAsync(@params);
        return new Wrapper(result);
    }

    [HttpGet("{id}")]
    public async Task<Wrapper> GetByIdAsync([FromRoute] int id)
    {
        var result = await bookMarkService.RetrieveByIdAsync(id);
        return new Wrapper(result);
    }

    [HttpDelete("{id}")]
    public async Task<Wrapper> DeleteAsync([FromRoute] int id)
    {
        var result = await bookMarkService.RemoveAsync(id);
        return new Wrapper(result);
    }

    [HttpPut("{id}")]
    public async Task<Wrapper> UpdateAsync([FromRoute] int id, [FromBody] BookMarkForUpdateDto dto)
    {
        var result = await bookMarkService.ModifyAsync(id, dto);
        return new Wrapper(result);
    }
}
