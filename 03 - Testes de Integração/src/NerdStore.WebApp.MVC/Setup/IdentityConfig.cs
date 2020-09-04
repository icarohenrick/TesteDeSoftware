using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NerdStore.WebApp.MVC.Data;

namespace NerdStore.WebApp.MVC.Setup
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfig(this IServiceCollection services, bool EhAPI)
        {
            if(EhAPI)
            {
                services.AddDefaultIdentity<IdentityUser>(
                options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            }
            else
            {
                services.AddDefaultIdentity<IdentityUser>(
                options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            }

            return services;
        }
    }
}
