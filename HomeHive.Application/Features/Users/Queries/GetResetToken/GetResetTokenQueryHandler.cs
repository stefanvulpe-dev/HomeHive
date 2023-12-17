using System.Security.Cryptography;
using HomeHive.Application.Contracts.Interfaces;
using HomeHive.Application.Contracts.Queries;
using HomeHive.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace HomeHive.Application.Features.Users.Queries.GetResetToken;

public class GetResetTokenQueryHandler(UserManager<User> userManager, IEmailService emailService): IQueryHandler<GetResetTokenQuery, GetResetTokenQueryResponse>
{
    public async Task<GetResetTokenQueryResponse> Handle(GetResetTokenQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString());
        
        if (user is null)
        {
            return new GetResetTokenQueryResponse
            {
                IsSuccess = false,
                Message = "User not found"
            };
        }
        
        var token = GenerateToken(64);
        
        user.ResetPasswordToken = token;
        user.ResetPasswordTokenExpires = DateTime.UtcNow.AddMinutes(15);
        
        var result = await userManager.UpdateAsync(user);
        
        if (!result.Succeeded)
        {
            return new GetResetTokenQueryResponse
            {
                IsSuccess = false,
                Message = "Failed to generate reset token"
            };
        }
        
        var emailResult = await emailService.SendEmailAsync(user.Email!, "Reset Password", $"Your reset token is {token}");
        
        if (!emailResult.IsSuccess)
        {
            return new GetResetTokenQueryResponse
            {
                IsSuccess = false,
                Message = "Failed to send email"
            };
        }
        
        return new GetResetTokenQueryResponse
        {
            IsSuccess = true,
            Message = "Reset token sent successfully"
        };
    }
    
    private static string GenerateToken(int length)
    {
        var randomNumber = new byte[length];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}