using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyICSv2._1.ViewModels;
using MyICSv2._1.Views;
using MyICSv2._1.Data;
using System;
using System.Diagnostics.Contracts;
using System.IO;

namespace MyICSv2._1;

public partial class App : Application
{
    public static IServiceProvider? Services { get; private set; } = null;
    public static IConfiguration? Configuration { get; private set; } = null;
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        Configuration = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(AppContext.BaseDirectory, "appsettings.json"), optional: false, reloadOnChange: false)
                .Build();

        var connectionString = Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' not found in appsettings.json.");

        var services = new ServiceCollection();

        services.AddDbContextFactory<AppDbContext>(options =>
            options.UseSqlite(connectionString));

        Services = services.BuildServiceProvider();

        //Apply any pending migrations at startup
        using (var context = Services.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext())
        {
            context.Database.Migrate();
        }



        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}