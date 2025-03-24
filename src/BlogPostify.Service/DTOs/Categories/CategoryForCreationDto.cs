using BlogPostify.Domain.Entities;

namespace BlogPostify.Service.DTOs.Categories;

public class CategoryForCreationDto
{
    public string Name { get; set; }
    public string Description { get; set; }
}
