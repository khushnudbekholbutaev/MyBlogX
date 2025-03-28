using BlogPostify.Domain.Commons;
using BlogPostify.Domain.Entities;
using BlogPostify.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Reflection;
using System.Text.Json;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        var converter = new ValueConverter<MultyLanguageField, string>(
            v => JsonSerializer.Serialize(v, options),
            v => JsonSerializer.Deserialize<MultyLanguageField>(v, options));

        // ======================== USER CONFIGURATION ========================
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(u => u.UserName).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(200);
            entity.Property(u => u.Password).IsRequired();
            entity.Property(u => u.Bio).HasMaxLength(500);
            entity.Property(u => u.ProfileImageUrl).HasMaxLength(300);

            entity.HasMany(u => u.Posts)
                  .WithOne(p => p.User)
                  .HasForeignKey(p => p.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(u => u.Comments)
                  .WithOne(c => c.User)
                  .HasForeignKey(c => c.UserId)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.HasMany(u => u.Likes)
                  .WithOne(l => l.User)
                  .HasForeignKey(l => l.UserId)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.HasMany(u => u.Bookmarks)
                  .WithOne(b => b.User)
                  .HasForeignKey(b => b.UserId)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.HasMany(u => u.Notifications)
                  .WithOne(n => n.User)
                  .HasForeignKey(n => n.UserId)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.HasMany(u => u.RefreshTokens)
                  .WithOne(rt => rt.User)
                  .HasForeignKey(rt => rt.UserId)
                  .OnDelete(DeleteBehavior.Cascade); // ✅ RefreshToken o‘chishi kerak

            entity.HasMany(u => u.UserRoles)
                  .WithOne(ur => ur.User)
                  .HasForeignKey(ur => ur.UserId)
                  .OnDelete(DeleteBehavior.NoAction);
        });

        // ======================== USER ROLE CONFIGURATION ========================
        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.Property(ur => ur.UserId).IsRequired();
            entity.Property(ur => ur.Role).HasConversion<int>().IsRequired();

            entity.HasOne(ur => ur.User)
                  .WithMany(u => u.UserRoles)
                  .HasForeignKey(ur => ur.UserId)
                  .OnDelete(DeleteBehavior.NoAction);
        });

        // ======================== REFRESH TOKEN CONFIGURATION ========================
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.Property(rt => rt.Token).IsRequired();
            entity.Property(rt => rt.ExpiryDate).IsRequired();

            entity.HasOne(rt => rt.User)
                  .WithMany(u => u.RefreshTokens)
                  .HasForeignKey(rt => rt.UserId)
                  .OnDelete(DeleteBehavior.Cascade); // ✅ To‘g‘ri ishlaydi
        });

        // ======================== POST CONFIGURATION ========================
        modelBuilder.Entity<Post>(entity =>
        {
            entity.Property(e => e.Title)
                  .HasConversion(converter)
                  .HasColumnType("nvarchar(max)");
            
            entity.Property(e => e.Content)
                  .HasConversion(converter)
                  .HasColumnType("nvarchar(max)");

            entity.Property(p => p.CoverImage).HasMaxLength(500);

            entity.HasOne(p => p.User)
                  .WithMany(u => u.Posts)
                  .HasForeignKey(p => p.UserId)
                  .OnDelete(DeleteBehavior.Cascade); // ✅ Post o‘chsa, hammasi o‘chadi

            entity.HasMany(p => p.Comments)
                  .WithOne(c => c.Post)
                  .HasForeignKey(c => c.PostId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(p => p.Likes)
                  .WithOne(l => l.Post)
                  .HasForeignKey(l => l.PostId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(p => p.Bookmarks)
                  .WithOne(b => b.Post)
                  .HasForeignKey(b => b.PostId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(p => p.PostCategories)
                  .WithOne(pc => pc.Post)
                  .HasForeignKey(pc => pc.PostId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(p => p.PostTags)
                  .WithOne(pt => pt.Post)
                  .HasForeignKey(pt => pt.PostId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ======================== COMMENT CONFIGURATION ========================
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.Property(c => c.Content).IsRequired();

            entity.HasOne(c => c.Post)
                  .WithMany(p => p.Comments)
                  .HasForeignKey(c => c.PostId)
                  .OnDelete(DeleteBehavior.Cascade); // Agar Post o'chirilsa, Comment ham o'chsin

            entity.HasOne(c => c.User)
                  .WithMany(u => u.Comments)
                  .HasForeignKey(c => c.UserId)
                  .OnDelete(DeleteBehavior.Restrict); // Agar User o'chirilsa, Comment ham o'chsin

            entity.HasOne(c => c.ParentComment)
                  .WithMany(c => c.Replies)
                  .HasForeignKey(c => c.ParentCommentId)
                  .OnDelete(DeleteBehavior.Restrict); // ✅ Javoblar saqlansin

            entity.Property(c => c.ParentCommentId).IsRequired(false);
        });

        // ======================== LIKE CONFIGURATION ========================
        modelBuilder.Entity<Like>(entity =>
        {
            entity.HasOne(l => l.Post)
                  .WithMany(p => p.Likes)
                  .HasForeignKey(l => l.PostId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(l => l.User)
                  .WithMany(u => u.Likes)
                  .HasForeignKey(l => l.UserId)
                  .OnDelete(DeleteBehavior.NoAction);
        });

        // ======================== BOOKMARK CONFIGURATION ========================
        modelBuilder.Entity<BookMark>(entity =>
        {
            entity.HasOne(b => b.User)
                  .WithMany(u => u.Bookmarks)
                  .HasForeignKey(b => b.UserId)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(b => b.Post)
                  .WithMany(p => p.Bookmarks)
                  .HasForeignKey(b => b.PostId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ======================== NOTIFICATION CONFIGURATION ========================
        modelBuilder.Entity<Notification>(entity =>
        {
            entity.Property(n => n.Message).IsRequired().HasMaxLength(500);
            entity.Property(n => n.IsRead).HasDefaultValue(false);

            entity.HasOne(n => n.User)
                  .WithMany(u => u.Notifications)
                  .HasForeignKey(n => n.UserId)
                  .OnDelete(DeleteBehavior.NoAction);
        });

        // ======================== CATEGORY CONFIGURATION ========================
        modelBuilder.Entity<Category>(entity =>
        {
            //entity.Property(e => e.Name)
            //      .IsRequired()
            //      .HasMaxLength(100);

            entity.Property(e => e.Name)
                  .HasConversion(converter)
                  .HasColumnType("nvarchar(max)");

            entity.Property(e => e.CreatedAt)
                  .HasColumnType("datetimeoffset")
                  .IsRequired();

            entity.Property(e => e.UpdatedAt)
                  .HasColumnType("datetimeoffset")
                  .IsRequired(false); // ✅ NULL bo‘lishi mumkin

            entity.HasMany(e => e.PostCategories)
                  .WithOne(pc => pc.Category)
                  .HasForeignKey(pc => pc.CategoryId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
