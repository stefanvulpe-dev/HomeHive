using HomeHive.Application.Contracts.Interfaces;
using HomeHive.Domain.Common;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace HomeHive.Infrastructure.Services.Email;

public class EmailService(IOptions<SendGridConfigurationOptions> options): IEmailService
{
    public async Task<Result> SendEmailAsync(string email, string subject, string message)
    {
        var client = new SendGridClient(options.Value.ApiKey);

        var from = new EmailAddress(options.Value.SenderEmail, "HomeHive");
        var to = new EmailAddress(email);

        var htmlContent = $"<strong>{message}</strong>";
        var msg = MailHelper.CreateSingleEmail(from, to, subject, "Your reset password code: ", htmlContent);

        var response = await client.SendEmailAsync(msg);

        return !response.IsSuccessStatusCode
            ? Result.Failure("Failed to send email")
            : Result.Success("Email sent successfully");
    }
}