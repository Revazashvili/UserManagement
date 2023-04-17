using System;
using System.Threading.Tasks;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace API;

/// <summary>
/// Entry Point Of Application
/// </summary>
public class Program
{
    /// <summary>
    /// Migrates pending database changes and starts the application
    /// </summary>
    public static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("serilog.json")
            .Build();
            
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        try
        {
            Log.Information("Application Starting...");
            var host = CreateHostBuilder(args).Build();
            using var scope = host.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            try
            {
                var context = serviceProvider.GetService<ApplicationDbContext>();
                if (context!.Database.IsSqlServer())
                    await context.Database.MigrateAsync();
                // Seed database here
            }
            catch (Exception e)
            {
                Log.Fatal(e,"An error occurred while migrating the database.");
                throw;
            }

            await host.RunAsync();
        }
        catch (Exception e)
        {
            Log.Fatal(e, "The Application failed to start...");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
        
    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args).UseSerilog().ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>());
}