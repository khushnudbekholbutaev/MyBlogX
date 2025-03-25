using BlogPostify.Domain.Entities.Users;
using BlogPostify.Service.DTOs.Auths;
using BlogPostify.Service.Interfaces.Auths;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System;
using BlogPostify.Service.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BlogPostify.Service.Services.Auths;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly DataContext appDbContext;
    private readonly IConfiguration configuration;

    public RefreshTokenService(DataContext appContext, IConfiguration configuration)
    {
        this.appDbContext = appContext;
        this.configuration = configuration;
    }
    public async Task<LoginResultDto> RefreshTokenAsync(RefreshTokenDto dto)
    {
        var storedToken = await appDbContext.Set<RefreshToken>()
            .Include(s => s.User)
            .FirstOrDefaultAsync(r => r.Token == dto.RefreshToken);

        if (storedToken == null || storedToken.ExpiryDate < DateTime.UtcNow)
            throw new BlogPostifyException(401, "Invalid or expired refresh token");

        var newAccessToken = GenerateToken(storedToken.User);
        var newRefreshToken = GenerateRefreshToken();

        storedToken.Token = newRefreshToken;
        storedToken.ExpiryDate = DateTime.UtcNow.AddMinutes(2);

        await appDbContext.SaveChangesAsync();

        return new LoginResultDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }

    // Yangi access token yaratish uchun metod
    private string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.UTF8.GetBytes(configuration["JWT:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Username", user.UserName),
                    new Claim("Email",user.Email)
            }),
            Audience = configuration["JWT:Audience"],
            Issuer = configuration["JWT:Issuer"],
            IssuedAt = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddSeconds(double.Parse(configuration["JWT:Expire"])),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    // Yangi refresh token yaratish uchun metod
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
        }
        return Convert.ToBase64String(randomNumber);
    }
}
