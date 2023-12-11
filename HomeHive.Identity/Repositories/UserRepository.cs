using HomeHive.Application.Persistence;
using HomeHive.Domain.Common;
using HomeHive.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HomeHive.Identity.Repositories;

public sealed class UserRepository(HomeHiveIdentityContext context, UserManager<User> userManager) : IUserRepository
{
    public async Task<Result<User>> FindByIdAsync(Guid id)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        return user == null ? Result<User>.Failure($"Entity with id {id} not found") : Result<User>.Success(user);
    }

    public async Task<Result<User>> UpdateAsync(User entity)
    {
        context.Entry(entity).State = EntityState.Modified;
        var result = await context.SaveChangesAsync();
        return result == 0 ? Result<User>.Failure("Entity could not be updated") : Result<User>.Success(entity);
    }

    public async Task<Result> DeleteByIdAsync(Guid id)
    {
        var userResult = await FindByIdAsync(id);
        if (!userResult.IsSuccess) return Result.Failure($"Entity with id {id} not found");
        
        var result = await userManager.DeleteAsync(userResult.Value);
        if (!result.Succeeded) return Result.Failure($"Entity with id {id} not found");

        return Result.Success();
    }

    public async Task<Result<IReadOnlyList<User>>> GetAllAsync()
    {
        var result = await context.Set<User>().ToListAsync();
        return Result<IReadOnlyList<User>>.Success(result);
    }
}