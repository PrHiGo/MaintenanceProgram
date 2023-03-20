using MaintenanceProgram.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MaintenanceProgram.Services;

internal abstract class GenericService<TEntity> where TEntity : class
{
    private readonly DataContext _context = new DataContext();

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _context.Set<TEntity>().ToListAsync();
    }

    public virtual async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var item = await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        {
            if (item != null)
            {
                return item;
            }
        }

        return null!;
    }

    public virtual async Task<TEntity> SaveEntityAsync(TEntity entity)
    {
        _context.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }


    public virtual async Task<TEntity> SaveEntityAsync(TEntity entity, Expression<Func<TEntity, bool>> alreadyExist)
    {
        var item = await GetSingleAsync(alreadyExist);
        if (item == null!)
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        return item;
    }

}
