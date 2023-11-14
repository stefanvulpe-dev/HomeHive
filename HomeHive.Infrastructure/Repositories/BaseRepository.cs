using HomeHive.Application.Persistence;
using HomeHive.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace HomeHive.Infrastructure.Repositories;

public class BaseRepository<T>: IAsyncRepository<T> where T : class
{
    public BaseRepository(HomeHiveContext context)
    {
        Context = context;
    }

    protected HomeHiveContext Context { get; }

    public virtual async Task<Result<T>> AddAsync(T entity)
    {
        var result = await Context.Set<T>().AddAsync(entity);
        if (result.State != EntityState.Added) return Result<T>.Failure($"Entity could not be added");
        var saveResult = await Context.SaveChangesAsync();
        return saveResult == 0 ? Result<T>.Failure($"Entity could not be added") : Result<T>.Success(entity);
    }

    public virtual async Task<Result<T>> UpdateAsync(T entity)
    {
        var result = await Context.SaveChangesAsync();
        return result == 0 ? Result<T>.Failure($"Entity could not be updated") : Result<T>.Success(entity);
    }

    public virtual async Task<Result<T>> FindByIdAsync(Guid id)
    {
        var result = await Context.Set<T>().FindAsync(id);
        return result == null ? Result<T>.Failure($"Entity with id {id} not found") : Result<T>.Success(result);
    }

    public virtual async Task<Result<T>> RemoveAsync(T entity)
    {
        Context.Set<T>().Remove(entity);
        var result = await Context.SaveChangesAsync();
        return result == 0 ? Result<T>.Failure($"Entity could not be deleted") : Result<T>.Success(entity);
    }

    public virtual async Task<Result<T>> DeleteByIdAsync(Guid id)
    {
        var result = await FindByIdAsync(id);
        if (result is not { IsSuccess: true }) return Result<T>.Failure($"Entity with id {id} not found");
        Context.Set<T>().Remove(result.Value);
        await Context.SaveChangesAsync();
        return Result<T>.Success(result.Value);
    }

    public virtual async Task<Result<IReadOnlyList<T>>> GetAllAsync()
    {
        var result = await Context.Set<T>().ToListAsync();
        return Result<IReadOnlyList<T>>.Success(result);
    }
}