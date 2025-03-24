using BlogPostify.Domain.Entities;
using BlogPostify.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection;
using System.Text.Json;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);

        // ======================== USER CONFIGURATION ========================
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(u => u.UserName).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(200);
            entity.Property(u => u.Password).IsRequired();
            entity.Property(u => u.Bio).HasMaxLength(500);
            entity.Property(u => u.ProfileImageUrl).HasMaxLength(300);

            // Navigation Properties
            entity.HasMany(u => u.Posts)
                  .WithOne(p => p.User)
                  .HasForeignKey(p => p.UserId);

            entity.HasMany(u => u.Comments)
                  .WithOne(c => c.User)
                  .HasForeignKey(c => c.UserId);

            entity.HasMany(u => u.Likes)
                  .WithOne(l => l.User)
                  .HasForeignKey(l => l.UserId);

            entity.HasMany(u => u.Bookmarks)
                  .WithOne(b => b.User)
                  .HasForeignKey(b => b.UserId);

            entity.HasMany(u => u.Notifications)
                  .WithOne(n => n.User)
                  .HasForeignKey(n => n.UserId);

            entity.HasMany<UserRole>()
                  .WithOne(ur => ur.User)
                  .HasForeignKey(ur => ur.UserId);
        });

        // ======================== USER ROLE CONFIGURATION ========================
        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.Property(ur => ur.UserId).IsRequired();

            // Enum (Role) int sifatida saqlanadi
            entity.Property(ur => ur.Role)
                  .HasConversion<int>() // Enumni int sifatida saqlash
                  .IsRequired();

            entity.HasOne(ur => ur.User)
                  .WithMany()
                  .HasForeignKey(ur => ur.UserId);
        });

        // ======================== POST CONFIGURATION ========================
        modelBuilder.Entity<Post>(entity =>
        {
            
            entity.Property(p => p.CoverImage)
                  .HasMaxLength(500);

            entity.HasOne(p => p.User)
                  .WithMany(u => u.Posts)
                  .HasForeignKey(p => p.UserId)
                  .OnDelete(DeleteBehavior.Restrict);

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

            // Post bilan bog'lanish
            entity.HasOne(c => c.Post)
                  .WithMany(p => p.Comments)
                  .HasForeignKey(c => c.PostId)
                  .OnDelete(DeleteBehavior.Restrict); // Agar Post o'chirilsa, Comment ham o'chsin

            // User bilan bog'lanish
            entity.HasOne(c => c.User)
                  .WithMany(u => u.Comments)
                  .HasForeignKey(c => c.UserId)
                  .OnDelete(DeleteBehavior.Restrict); // Agar User o'chirilsa, Comment ham o'chsin

            // ParentComment (ota kommentariya) bilan bog'lanish
            entity.HasOne(c => c.ParentComment)
                  .WithMany(c => c.Replies)
                  .HasForeignKey(c => c.ParentCommentId)
                  .OnDelete(DeleteBehavior.Restrict); // Agar ota kommentariya o'chirilsa, javoblar o'chmasin

            // ParentCommentId ni nullable qilish
            entity.Property(c => c.ParentCommentId)
                  .IsRequired(false); // Null bo'lishi mumkin
        });
        // ======================== LIKE CONFIGURATION ========================
        modelBuilder.Entity<Like>(entity =>
        {
            // Navigation Properties
            entity.HasOne(l => l.Post)
                  .WithMany(p => p.Likes)
                  .HasForeignKey(l => l.PostId);

            entity.HasOne(l => l.User)
                  .WithMany(u => u.Likes)
                  .HasForeignKey(l => l.UserId);
        });

        // ======================== BOOKMARK CONFIGURATION ========================
        modelBuilder.Entity<BookMark>(entity =>
        {
            // Navigation Properties
            entity.HasOne(b => b.User)
                  .WithMany(u => u.Bookmarks)
                  .HasForeignKey(b => b.UserId);

            entity.HasOne(b => b.Post)
                  .WithMany(p => p.Bookmarks)
                  .HasForeignKey(b => b.PostId);
        });

        // ======================== NOTIFICATION CONFIGURATION ========================
        modelBuilder.Entity<Notification>(entity =>
        {
            entity.Property(n => n.Message).IsRequired().HasMaxLength(500);
            entity.Property(n => n.IsRead).HasDefaultValue(false);

            // Navigation Properties
            entity.HasOne(n => n.User)
                  .WithMany(u => u.Notifications)
                  .HasForeignKey(n => n.UserId);
        });
    }
}
