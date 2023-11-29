using System.Text;
using HomeHive.Application.Persistence;
using HomeHive.Domain.Entities;
using HomeHive.Identity.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace HomeHive.Identity;

public static class IdentityRegistrationDI
{
    public static void AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<HomeHiveIdentityContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("HomeHiveIdentityConnection"),
                builder => builder.MigrationsAssembly(typeof(HomeHiveIdentityContext).Assembly.FullName)));
        services.AddIdentity<User, IdentityRole<Guid>>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<HomeHiveIdentityContext>()
            .AddDefaultTokenProviders();
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = configuration["JWT:ValidAudience"],
                    ValidIssuer = configuration["JWT:ValidIssuer"],
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!))
                };
            });
        services.AddScoped<IUserRepository, UserRepository>();
    }
}