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

    [HttpGet]
    public async Task<Wrapper> GetAllAsync([FromQuery] PaginationParams @params, [FromQuery] string? language = "uz")
    {
        var result = await postService.RetrieveAllAsync(@params);

        // Foydalanuvchi tanlagan til bo‘yicha ma'lumotni qaytarish
        var filteredResult = result.Select(post => new
        {
            post.Id,
            Title = post.Translations.ContainsKey(language) ? post.Translations[language].Title : post.Translations["uz"].Title,
            Content = post.Translations.ContainsKey(language) ? post.Translations[language].Content : post.Translations["uz"].Content,
            post.CoverImage,
            post.UserId,
            post.IsPublished
        });

        return new Wrapper(filteredResult);
    }

    [HttpGet("{id}")]
    public async Task<Wrapper> GetByIdAsync([FromRoute] int id, [FromQuery] string? language = "uz")
    {
        var result = await postService.RetrieveIdAsync(id);

        // Foydalanuvchi tanlagan til bo‘yicha ma'lumotni qaytarish
        var filteredResult = new
        {
            result.Id,
            Title = result.Translations.ContainsKey(language) ? result.Translations[language].Title : result.Translations["uz"].Title,
            Content = result.Translations.ContainsKey(language) ? result.Translations[language].Content : result.Translations["uz"].Content,
            result.CoverImage,
            result.UserId,
            result.IsPublished
        };

        return new Wrapper(filteredResult);
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
