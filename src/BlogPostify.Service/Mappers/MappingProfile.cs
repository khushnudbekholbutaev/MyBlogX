using AutoMapper;
using BlogPostify.Domain.Entities;
using BlogPostify.Domain.Entities.Users;
using BlogPostify.Service.DTOs.BookMarks;
using BlogPostify.Service.DTOs.Categories;
using BlogPostify.Service.DTOs.Comments;
using BlogPostify.Service.DTOs.Likes;
using BlogPostify.Service.DTOs.Notifications;
using BlogPostify.Service.DTOs.PostCategories;
using BlogPostify.Service.DTOs.Posts;
using BlogPostify.Service.DTOs.PostTags;
using BlogPostify.Service.DTOs.Tags;
using BlogPostify.Service.DTOs.UserRoles;
using BlogPostify.Service.DTOs.Users;
using System.Runtime.CompilerServices;

namespace BlogPostify.Service.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Folder Name :  User
        CreateMap<User, UserForUpdateDto>().ReverseMap();
        CreateMap<User, UserForResultDto>().ReverseMap();
        CreateMap<User, UserForCreationDto>().ReverseMap();

        // Folder Name : UserRole
        CreateMap<UserRole,UserRoleForCreationDto>().ReverseMap();
        CreateMap<UserRole, UserRoleForResultDto>().ReverseMap();
        CreateMap<UserRole, UserRoleForUpdateDto>().ReverseMap();

        // Folder Name : Post
        CreateMap<Post,PostForCreationDto>().ReverseMap();
        CreateMap<Post,PostForResultDto>().ReverseMap();
        CreateMap<Post, PostForUpdateDto>().ReverseMap();

        // Folder Name : Category
        CreateMap<Category,CategoryForCreationDto>().ReverseMap();
        CreateMap<Category,CategoryForResultDto>().ReverseMap();
        CreateMap<Category,CategoryForUpdateDto>().ReverseMap();

        // Folder Name : Tag
        CreateMap<Tag,TagForCreationDto>().ReverseMap();
        CreateMap<Tag,TagFoResultDto>().ReverseMap();
        CreateMap<Tag,TagForUpdateDto>().ReverseMap();

        // Folder Name : PostCategory
        CreateMap<PostCategory, PostCategoryForCreationDto>().ReverseMap();
        CreateMap<PostCategory,PostCategoryForResultDto>().ReverseMap();
        CreateMap<PostCategory,PostCategoryForUpdateDto>().ReverseMap();

        // Folder Name : PostTag
        CreateMap<PostTag, PostTagForCreationDto>().ReverseMap();
        CreateMap<PostTag,PostTagForResultDto>().ReverseMap();
        CreateMap<PostTag, PostTagForUpdateDto>().ReverseMap();

        // Folder Name : Comment
        CreateMap<Comment,CommentForCreationDto>().ReverseMap();
        CreateMap<Comment,CommentForResultDto>().ReverseMap();
        CreateMap<Comment,CommentForUpdateDto>().ReverseMap();

        // Folder Name : Like
        CreateMap<Like,LikeForCreationDto>().ReverseMap();
        CreateMap<Like, LikeForResultDto>().ReverseMap();
        CreateMap<Like,LikeForUpdateDto>().ReverseMap();

        // Fodler Name : BookMark
        CreateMap<BookMark,BookMarkForCreationDto>().ReverseMap();
        CreateMap<BookMark,BookMarkForResultDto>().ReverseMap();
        CreateMap<BookMark,BookMarkForUpdateDto>().ReverseMap();    

        // Folder Name : Notification
        CreateMap<Notification,NotificationForCreationDto>().ReverseMap();
        CreateMap<Notification,NotificationForResultDto>().ReverseMap();
        CreateMap<Notification,NotificationForUpdateDto>().ReverseMap();
    }
}
