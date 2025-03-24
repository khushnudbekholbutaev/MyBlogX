﻿using AutoMapper;
using BlogPostify.Data.IRepositories;
using BlogPostify.Domain.Configurations;
using BlogPostify.Domain.Entities;
using BlogPostify.Service.Commons.CollectionExtensions;
using BlogPostify.Service.DTOs.Categories;
using BlogPostify.Service.Exceptions;
using BlogPostify.Service.Interfaces.Categories;
using Microsoft.EntityFrameworkCore;

namespace BlogPostify.Service.Services.Categories;

public class CategoryService : ICategoryService
{
    private readonly IMapper mapper;
    private readonly IRepository<Category,int> repository;

    public CategoryService(
        IMapper mapper, 
        IRepository<Category, int> repository)
    {
        this.mapper = mapper;
        this.repository = repository;
    }

    public async Task<CategoryForResultDto> AddAsync(CategoryForCreationDto dto)
    {
        
        if (await repository.SelectAll().AnyAsync(c => c.Name.ToLower() == dto.Name.ToLower()))
            throw new BlogPostifyException(409, "Category is already exists");

        var mapped = mapper.Map<Category>(dto);
        mapped.CreatedAt = DateTime.UtcNow;
        await repository.InsertAsync(mapped);

        return mapper.Map<CategoryForResultDto>(mapped);
    }

    public async Task<CategoryForResultDto> ModifyAsync(int id, CategoryForUpdateDto dto)
    {
        var category = await repository.SelectAll()
                                                    .Where(category => category.Id == id)
                                                    .FirstOrDefaultAsync()
            ?? throw new BlogPostifyException(404, "Category is not found");

        var mapped = mapper.Map(dto,category);
        mapped.UpdatedAt = DateTime.UtcNow; 
        await repository.UpdateAsync(mapped);

        return mapper.Map<CategoryForResultDto>(mapped);
    }

    public async Task<bool> RemoveAsync(int id)
    {
        if (await repository.SelectAll().AnyAsync(category => category.Id == id))
            throw new BlogPostifyException(404, "Category is not found");

        await repository.DeleteAsync(id);
        return true;
    }

    public async Task<IEnumerable<CategoryForResultDto>> RetrieveAllAsync(PaginationParams @params)
    {
        var categories = await repository.SelectAll()
                                                     .ToPagedList(@params)
                                                     .AsNoTracking()
                                                     .ToListAsync();
        return mapper.Map<IEnumerable<CategoryForResultDto>>(categories);
    }

    public async Task<CategoryForResultDto> RetrieveByIdAsync(int id)
    {
        var category = await repository.SelectAll()
                                                    .Where(category => category.Id == id)
                                                    .FirstOrDefaultAsync()
            ?? throw new BlogPostifyException(404, "Category is not found");
        return mapper.Map<CategoryForResultDto>(category);
    }
}
