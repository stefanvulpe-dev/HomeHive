namespace HomeHive.Infrastructure.Services.Email;

public class SendGridConfigurationOptions
{
    public string? ApiKey { get; set; }
    public string? SenderEmail { get; set; }
}