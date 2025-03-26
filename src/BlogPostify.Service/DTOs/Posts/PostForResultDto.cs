﻿using BlogPostify.Domain.Commons;
using BlogPostify.Domain.Entities;

namespace BlogPostify.Service.DTOs.Posts;

// 4  : 1 , 3 , 2
public class PostForResultDto
{
    public int Id { get; set; }
    public MultyLanguageField Title { get; set; }
    public MultyLanguageField Content { get; set; }
    public string CoverImage { get; set; } = "string";
    public int UserId { get; set; }
    public bool IsPublished { get; set; }
}
