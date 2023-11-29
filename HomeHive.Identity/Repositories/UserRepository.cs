using HomeHive.Application.Persistence;
using HomeHive.Domain.Common;
using HomeHive.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomeHive.Identity.Repositories;

public class UserRepository : IUserRepository
{
    private readonly HomeHiveIdentityContext _context;

    public UserRepository(HomeHiveIdentityContext context)
    {
        _context = context;
    }

    public async Task<Result<User>> GetByEmailAsync(string email)
    {
        var result = await _context.Set<User>().FirstOrDefaultAsync(u => u.Email == email);
        return result == null
            ? Result<User>.Failure($"Entity with email {email} not found")
            : Result<User>.Success(result);
    }

    public async Task<Result<User>> AddAsync(User entity)
    {
        var result = await _context.Set<User>().AddAsync(entity);
        if (result.State != EntityState.Added) return Result<User>.Failure("Entity could not be added");
        var saveResult = await _context.SaveChangesAsync();
        return saveResult == 0 ? Result<User>.Failure("Entity could not be added") : Result<User>.Success(entity);
    }

    public virtual async Task<Result<User>> UpdateAsync(User entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        var result = await _context.SaveChangesAsync();
        return result == 0 ? Result<User>.Failure("Entity could not be updated") : Result<User>.Success(entity);
    }

    public virtual async Task<Result<User>> FindByIdAsync(Guid id)
    {
        var result = await _context.Set<User>().FindAsync(id);
        return result == null ? Result<User>.Failure($"Entity with id {id} not found") : Result<User>.Success(result);
    }

    public virtual async Task<Result> RemoveAsync(User entity)
    {
        _context.Set<User>().Remove(entity);
        var result = await _context.SaveChangesAsync();
        return result == 0 ? Result.Failure("Entity could not be deleted") : Result.Success();
    }

    public virtual async Task<Result> DeleteByIdAsync(Guid id)
    {
        var result = await FindByIdAsync(id);
        if (result is not { IsSuccess: true }) return Result.Failure($"Entity with id {id} not found");
        _context.Set<User>().Remove(result.Value);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public virtual async Task<Result<IReadOnlyList<User>>> GetAllAsync()
    {
        var result = await _context.Set<User>().ToListAsync();
        return Result<IReadOnlyList<User>>.Success(result);
    }
}