using BlogPostify.Data.IRepositories;
using Microsoft.EntityFrameworkCore;
using BlogPostify.Domain.Commons;
namespace BlogPostify.Data.Repositories;

public class Repository<TEntity, T> : IRepository<TEntity, T>
    where TEntity : BaseModel<T>
{
    private readonly DataContext _dbContext;
    private readonly DbSet<TEntity> _dbSet;

    public Repository(DataContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _dbSet = _dbContext.Set<TEntity>();
    }

    public async Task<TEntity> InsertAsync(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        var entry = await _dbSet.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return entry.Entity;
    }

    public async Task<bool> DeleteAsync(T id)
    {
        var entity = await _dbSet.FirstOrDefaultAsync(e => e.Id.Equals(id));
        if (entity == null)
            return false;

        _dbSet.Remove(entity);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public IQueryable<TEntity> SelectAll()
        => _dbSet.AsNoTracking();

    public async Task<TEntity?> SelectByIdAsync(T id)
        => await _dbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id.Equals(id));

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        var existingEntity = await _dbSet.FindAsync(entity.Id);
        if (existingEntity == null)
            throw new InvalidOperationException("Entity not found");

        _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
        await _dbContext.SaveChangesAsync();
        return existingEntity;
    }
}

