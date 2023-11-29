using HomeHive.Domain.Common;

namespace HomeHive.Application.Persistence;

public interface IAsyncRepository<T> where T : class
{
    public Task<Result<T>> AddAsync(T entity);

    public Task<Result<T>> UpdateAsync(T entity);

    public Task<Result<T>> FindByIdAsync(Guid id);

    public Task<Result> RemoveAsync(T entity);

    public Task<Result> DeleteByIdAsync(Guid id);

    public Task<Result<IReadOnlyList<T>>> GetAllAsync();
}