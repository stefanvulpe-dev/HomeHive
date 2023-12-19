using HomeHive.Domain.Common;

namespace HomeHive.Application.Contracts.Interfaces;

public interface IEmailService
{
    Task<Result> SendEmailAsync(string email, string subject, string message);
}