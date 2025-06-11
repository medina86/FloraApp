using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FloraApp.Services.Database
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection AddFloraAppDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<FloraAppDbContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }
    }
} 