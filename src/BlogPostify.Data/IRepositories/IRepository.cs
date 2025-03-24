using BlogPostify.Domain.Commons;
using System.Security.AccessControl;

namespace BlogPostify.Data.IRepositories;

public interface IRepository<TEntity,T> where TEntity : BaseModel<T>
{
    IQueryable<TEntity> SelectAll();
    Task<bool> DeleteAsync(T id);
    Task<TEntity> SelectByIdAsync(T id);
    Task<TEntity> InsertAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
}