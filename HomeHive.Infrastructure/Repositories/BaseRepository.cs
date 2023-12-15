using HomeHive.Application.Persistence;
using HomeHive.Domain.Common;
using HomeHive.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomeHive.Infrastructure.Repositories;

public class BaseRepository<T> : IAsyncRepository<T> where T : BaseEntity
{
    private readonly HomeHiveContext _context;

    public BaseRepository(HomeHiveContext context)
    {
        _context = context;
    }

    public virtual async Task<Result<T>> AddAsync(T entity)
    {
        var result = await _context.Set<T>().AddAsync(entity);
        if (result.State != EntityState.Added) return Result<T>.Failure("Entity could not be added");
        var saveResult = await _context.SaveChangesAsync();
        return saveResult == 0 ? Result<T>.Failure("Entity could not be added") : Result<T>.Success(entity);
    }

    public virtual async Task<Result<T>> UpdateAsync(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        var result = await _context.SaveChangesAsync();
        return result == 0 ? Result<T>.Failure("Entity could not be updated") : Result<T>.Success(entity);
    }

    public virtual async Task<Result<T>> FindByIdAsync(Guid id)
    {
        var query = _context.Set<T>().AsQueryable();

        if (typeof(Estate) == typeof(T))
        {
            query = query.Include(x => ((Estate)(object)x).Utilities);
        }

        var result = await query.FirstOrDefaultAsync(x => EF.Property<Guid>(x, "Id") == id);

        return result == null
            ? Result<T>.Failure($"Entity with id {id} not found")
            : Result<T>.Success(result);
    }

    public virtual async Task<Result> RemoveAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        var result = await _context.SaveChangesAsync();
        return result == 0 ? Result.Failure("Entity could not be deleted") : Result.Success();
    }

    public virtual async Task<Result> DeleteByIdAsync(Guid id)
    {
        var result = await FindByIdAsync(id);
        if (result is not { IsSuccess: true }) return Result.Failure($"Entity with id {id} not found");
        _context.Set<T>().Remove(result.Value);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public virtual async Task<Result<IReadOnlyList<T>>> GetAllAsync()
    {
        var query = _context.Set<T>().AsQueryable();

        if (typeof(Estate) == typeof(T))
        {
            query = query.Include(x => ((Estate)(object)x).Utilities);
        }

        var result = await query.ToListAsync();

        return Result<IReadOnlyList<T>>.Success(result);
    }
}