using BlogPostify.Domain.Commons;
using BlogPostify.Domain.Entities;

namespace BlogPostify.Service.DTOs.Categories;

public class CategoryForCreationDto
{
    public MultyLanguageField Name { get; set; }
    //public string Description { get; set; }
}
