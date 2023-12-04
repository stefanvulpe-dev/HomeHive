namespace HomeHive.Application.Contracts.Interfaces;

public interface ICurrentUserService
{
    Guid GetCurrentUserId();
    string GetCurrentUserName();
}