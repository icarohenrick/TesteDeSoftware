using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NerdStore.WebApp.MVC.Models;
using System.Text;

namespace NerdStore.WebApp.MVC.Setup
{
    public static class JsonWebToken
    {
        public static IServiceCollection AddJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettingsSections = configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSections);

            var appsettings = appSettingsSections.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appsettings.Secret);

            services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(_ =>
            {
                _.RequireHttpsMetadata = false;
                _.SaveToken = true;
                _.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = appsettings.ValidoEm,
                    ValidIssuer = appsettings.Emissor
                };
            });

            return services;
        }
    }
}
