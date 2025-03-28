using AutoMapper;
using BlogPostify.Data.IRepositories;
using BlogPostify.Domain.Commons;
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
        
        if (await repository.SelectAll().AnyAsync(c => c.Name == dto.Name))
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

    public async Task<LanguageResultDto> RetrieveByLanguageAsync(int id, string language)
    {
        var ctr = await repository.SelectAll()
              .Where(p => p.Id == id)
              .FirstOrDefaultAsync();

        if (ctr == null)
            throw new KeyNotFoundException($"Category with ID {id} not found!");

        if (ctr.Name == null)
            throw new InvalidOperationException("Post Name is null!");

        string name = GetLocalizedText(ctr.Name, language);

        return new LanguageResultDto
        {
            Id = ctr.Id,
            Name = name,
        };
    }

    private string GetLocalizedText(MultyLanguageField field, string language)
    {
        if (field == null)
            return "No translation available";

        return language switch
        {
            "uz" => !string.IsNullOrEmpty(field.Uz) ? field.Uz : field.Eng,
            "ru" => !string.IsNullOrEmpty(field.Ru) ? field.Ru : field.Eng,
            "eng" => !string.IsNullOrEmpty(field.Eng) ? field.Eng : field.Uz,
            "tr" => !string.IsNullOrEmpty(field.Tr) ? field.Tr : field.Eng,
            _ => field.Uz
        };
    }
}
