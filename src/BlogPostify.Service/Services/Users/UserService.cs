using AutoMapper;
using BlogPostify.Data.IRepositories;
using BlogPostify.Domain.Configurations;
using BlogPostify.Domain.Entities.Users;
using BlogPostify.Service.Commons.CollectionExtensions;
using BlogPostify.Service.Commons.Helpers;
using BlogPostify.Service.DTOs.Users;
using BlogPostify.Service.Exceptions;
using BlogPostify.Service.Interfaces.Users;
using Microsoft.EntityFrameworkCore;

namespace BlogPostify.Service.Services.Users;

public class UserService : IUserService
{
    private readonly IMapper mapper;
    private readonly IRepository<User,int> userRepository;
    public UserService(IMapper mapper, IRepository<User,int> userRepository)
    {
        this.mapper = mapper;
        this.userRepository = userRepository;
    }

    public async Task<UserForResultDto> AddAsync(UserForCreationDto dto)
    {
        var user = await userRepository.SelectAll()
            .Where(u => u.Email == dto.Email)
            .AsNoTracking()
            .FirstOrDefaultAsync();
        if (user is not null)
            throw new BlogPostifyException(409, "User is already exists");

        #region Image
        var imageFileName = Guid.NewGuid().ToString("N") + Path.GetExtension(dto.ProfileImageUrl.FileName);
        var imageRootPath = Path.Combine(WebEnvironmentHost.WebRootPath,
            "Media", "Users", "Images", imageFileName);
        using (var stream = new FileStream(imageRootPath, FileMode.Create))
        {
            await dto.ProfileImageUrl.CopyToAsync(stream);
            await stream.FlushAsync();
            stream.Close();
        }
        string imageResult = Path.Combine("Media", "Users", "Images", imageFileName);
        #endregion

        var mapped = mapper.Map<User>(dto);
        mapped.CreatedAt = DateTime.UtcNow;
        mapped.ProfileImageUrl = imageResult;
        await userRepository.InsertAsync(mapped);

        return mapper.Map<UserForResultDto>(mapped);
    }

    public async Task<UserForResultDto> ModifyAsync(int id, UserForUpdateDto dto)
    {
        var user = await userRepository.SelectAll()
           .Where(u => u.Id == id)
           .AsNoTracking()
           .FirstOrDefaultAsync()
            ??throw new BlogPostifyException(404, "User is not found");

        #region Image
        var imageFullPath = Path.Combine(WebEnvironmentHost.WebRootPath, user.ProfileImageUrl);

        if (File.Exists(imageFullPath))
            File.Delete(imageFullPath);

        var imageFileName = Guid.NewGuid().ToString("N") + Path.GetExtension(dto.ProfileImageUrl.FileName);
        var imageRootPath = Path.Combine(WebEnvironmentHost.WebRootPath, "Media", "Users", "Images", imageFileName);
        using (var stream = new FileStream(imageRootPath, FileMode.Create))
        {
            await dto.ProfileImageUrl.CopyToAsync(stream);
            await stream.FlushAsync();
            stream.Close();
        }
        string imageResult = Path.Combine("Media", "Users", "Images", imageFileName);
        #endregion

        var mapped = mapper.Map(dto, user);
        mapped.UpdatedAt = DateTime.UtcNow;
        mapped.ProfileImageUrl = imageResult;
        await userRepository.UpdateAsync(mapped);

        return mapper.Map<UserForResultDto>(mapped);
    }

    public async Task<bool> RemoveAsync(int id)
    {
        var user = await userRepository.SelectAll()
            .Where(u => u.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (user is null)
            throw new BlogPostifyException(404, "User is not found");

        #region Image
        var imageFullPath = Path.Combine(WebEnvironmentHost.WebRootPath, user.ProfileImageUrl);

        if (File.Exists(imageFullPath))
            File.Delete(imageFullPath);
        #endregion

        await userRepository.DeleteAsync(id);
        return true;
    }

    public async Task<IEnumerable<UserForResultDto>> RetrieveAllAsync(PaginationParams @params)
    {
        var users = await userRepository.SelectAll()
            .Include(u => u.Posts)
            .Include(u => u.Comments)
            .Include(u => u.Bookmarks)
            .ToPagedList(@params)
             .AsNoTracking()
             .ToListAsync();

        if (users is null)
            throw new BlogPostifyException(404, "User is not found!");

        return mapper.Map<IEnumerable<UserForResultDto>>(users);
    }
    
    public async Task<UserForResultDto> RetrieveByIdasync(int id)
    {
        var user = await userRepository.SelectAll()
               .Where(u => u.Id == id)
               .AsNoTracking()
               .FirstOrDefaultAsync();

        if (user is null)
            throw new BlogPostifyException(404, "User is not found!");

        return mapper.Map<UserForResultDto>(user);
    }
}

