using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace FloraApp.Services.Database
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<FloraAppDbContext>
    {
        public FloraAppDbContext CreateDbContext(string[] args)
        {
            // Get the configuration from appsettings.json in the WebAPI project
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../FloraAppWebAPI"))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            var configuration = configurationBuilder.Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<FloraAppDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new FloraAppDbContext(optionsBuilder.Options);
        }
    }
} 