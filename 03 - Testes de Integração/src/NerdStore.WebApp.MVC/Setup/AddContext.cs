using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NerdStore.Catalogo.Data;
using NerdStore.Vendas.Data;
using NerdStore.WebApp.MVC.Data;

namespace NerdStore.WebApp.MVC.Setup
{
    public static class AddContext
    {
        public static void AddContexts(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CatalogoContext>(options =>
               options.UseSqlServer(
                   configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<VendasContext>(options =>
               options.UseSqlServer(
                   configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
