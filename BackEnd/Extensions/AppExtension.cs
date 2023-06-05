using System.Text;
using BackEnd.Data;
using BackEnd.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BackEnd.Extensions;

public static class AppExtension
{
    public static void LoadConfiguration(this WebApplicationBuilder builder)
    {
        Configuration.JwtKey = builder.Configuration.GetValue<string>("JwtKey");
        Configuration.ApiKeyName = builder.Configuration.GetValue<string>("ApiKeyName");
        Configuration.ApiKey = builder.Configuration.GetValue<string>("ApiKey");

        var smtp = new Configuration.SmtpConfiguration();
        builder.Configuration.GetSection("SmtpConfiguration").Bind(smtp);
        Configuration.Smtp = smtp;
    }

    public static void ConfigureAuthentication(this WebApplicationBuilder builder)
    {
        var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);
        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });
    }
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(connectionString));
        builder.Services.AddTransient<EmailService>();
    }
}