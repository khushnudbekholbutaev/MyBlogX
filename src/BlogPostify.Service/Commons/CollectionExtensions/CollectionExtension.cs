using BlogPostify.Domain.Commons;
using BlogPostify.Domain.Configurations;
using BlogPostify.Domain.Entities.Users;
using BlogPostify.Service.Commons.Helpers;
using BlogPostify.Service.Exceptions;
using Newtonsoft.Json;

namespace BlogPostify.Service.Commons.CollectionExtensions;

//public static class CollectionExtension
//{
//    public static IQueryable<TEntity> ToPagedList<TEntity>(this IQueryable<TEntity> entities, PaginationParams @params)
//            where TEntity : BaseModel<long>
//    {
//        var metaData = new PaginationMetaData(entities.Count(), @params);

//        var json = JsonConvert.SerializeObject(metaData);

//        if (HttpContextHelper.ResponseHeaders != null)
//        {
//            if (HttpContextHelper.ResponseHeaders.ContainsKey("X-Pagination"))
//                HttpContextHelper.ResponseHeaders.Remove("X-Pagination");

//            HttpContextHelper.ResponseHeaders.Add("X-Pagination", json);
//        }

//        return @params.PageIndex > 0 && @params.PageSize > 0 ?
//            entities.OrderBy(e => e.Id)
//                .Skip((@params.PageIndex - 1) * @params.PageSize).Take(@params.PageSize) :
//                    throw new BlogPostifyException(400, "Please, enter valid numbers");
//    }
//    public static IQueryable<User> ToPagedList(this IQueryable<User> source, PaginationParams @params)
//    {

//        var metaData = new PaginationMetaData(source.Count(), @params);

//        var json = JsonConvert.SerializeObject(metaData);
//        if (HttpContextHelper.ResponseHeaders != null)
//        {
//            if (HttpContextHelper.ResponseHeaders.ContainsKey("X-Pagination"))
//                HttpContextHelper.ResponseHeaders.Remove("X-Pagination");

//            HttpContextHelper.ResponseHeaders.Add("X-Pagination", json);
//        }

//        return @params.PageIndex > 0 && @params.PageSize > 0 ?
//            source
//            .OrderBy(s => s.Id)
//            .Skip((@params.PageIndex - 1) * @params.PageSize).Take(@params.PageSize)
//            : throw new BlogPostifyException(400, "Please, enter valid numbers");
//    }

//    public static IEnumerable<TEntity> ToPagedList<TEntity>(this IEnumerable<TEntity> source, PaginationParams @params)
//    {
//        if (@params.PageIndex < 1)
//        {
//            throw new ArgumentOutOfRangeException(nameof(@params.PageIndex), "The page index must be greater than or equal to 1.");
//        }

//        if (@params.PageSize < 1)
//        {
//            throw new ArgumentOutOfRangeException(nameof(@params.PageSize), "The page size must be greater than or equal to 1.");
//        }

//        return source.Take((@params.PageSize * (@params.PageIndex - 1))..(@params.PageSize * (@params.PageIndex - 1) + @params.PageSize));
//    }
//}
public static class CollectionExtension
{
    public static IQueryable<TEntity> ToPagedList<TEntity>(this IQueryable<TEntity> entities, PaginationParams @params)
        where TEntity : BaseModel<int>
    {
        var metaData = new PaginationMetaData(entities.Count(), @params);
        var json = JsonConvert.SerializeObject(metaData);

        if (HttpContextHelper.ResponseHeaders != null)
        {
            if (HttpContextHelper.ResponseHeaders.ContainsKey("X-Pagination"))
                HttpContextHelper.ResponseHeaders.Remove("X-Pagination");

            HttpContextHelper.ResponseHeaders.Add("X-Pagination", json);
        }

        if (@params.PageIndex < 1 || @params.PageSize < 1)
            throw new BlogPostifyException(400, "Please, enter valid numbers");

        return entities
            .OrderBy(e => e.Id)
            .Skip((@params.PageIndex - 1) * @params.PageSize)
            .Take(@params.PageSize);
    }
    public static IQueryable<User> ToPagedList(this IQueryable<User> source, PaginationParams @params)
    {

        var metaData = new PaginationMetaData(source.Count(), @params);

        var json = JsonConvert.SerializeObject(metaData);
        if (HttpContextHelper.ResponseHeaders != null)
        {
            if (HttpContextHelper.ResponseHeaders.ContainsKey("X-Pagination"))
                HttpContextHelper.ResponseHeaders.Remove("X-Pagination");

            HttpContextHelper.ResponseHeaders.Add("X-Pagination", json);
        }

        return @params.PageIndex > 0 && @params.PageSize > 0 ?
            source
            .OrderBy(s => s.Id)
            .Skip((@params.PageIndex - 1) * @params.PageSize).Take(@params.PageSize)
            : throw new BlogPostifyException(400, "Please, enter valid numbers");
    }
    public static IEnumerable<TEntity> ToPagedList<TEntity>(this IEnumerable<TEntity> source, PaginationParams @params)
    {
        if (@params.PageIndex < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(@params.PageIndex), "The page index must be greater than or equal to 1.");
        }

        if (@params.PageSize < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(@params.PageSize), "The page size must be greater than or equal to 1.");
        }

        return source.Take((@params.PageSize * (@params.PageIndex - 1))..(@params.PageSize * (@params.PageIndex - 1) + @params.PageSize));
    }

}
