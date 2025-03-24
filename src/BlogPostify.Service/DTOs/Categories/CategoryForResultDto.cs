﻿using BlogPostify.Domain.Entities;

namespace BlogPostify.Service.DTOs.Categories;

public class CategoryForResultDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    // Relations
    public ICollection<PostCategory> PostCategories { get; set; }
}
