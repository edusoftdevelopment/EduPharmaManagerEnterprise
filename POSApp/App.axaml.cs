using System.Linq;
using System;
using System.Threading;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using POSApp.Data;
using POSApp.Factories;
using POSApp.Helpers;
using POSApp.Services;
using POSApp.ViewModels;
using POSApp.Views;
using Microsoft.Extensions.DependencyInjection;

namespace POSApp;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var services = new ServiceCollection();

        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<AppStateViewModel>();
        services.AddSingleton<AppConfigService>();

        services.AddScoped<DbHelper>();
        services.AddScoped<DropdownService>();
        services.AddScoped<ILoginService, LoginService>();

        services.AddTransient<LoginWindowViewModel>();
        services.AddTransient<EstimationInfoViewModel>();

        services.AddSingleton<Func<ApplicationPageNames, PageViewModel>>(x => name => name switch
        {
            ApplicationPageNames.EstimationInfo => x.GetRequiredService<EstimationInfoViewModel>(),
            _ => throw new ArgumentOutOfRangeException(nameof(name), name, null)
        });
        services.AddSingleton<PageFactory>();

        var provider = services.BuildServiceProvider();

        var configService = provider.GetRequiredService<AppConfigService>();
        var hasConnection = configService.Load();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            DisableAvaloniaDataAnnotationValidation();

            if (hasConnection)
            {
                var dbHelper = provider.GetRequiredService<DbHelper>();
                dbHelper.SetConnectionString(configService.ConnectionString);

                var appStateViewModel = provider.GetRequiredService<AppStateViewModel>();
                if (appStateViewModel.User is null)
                {
                    var loginViewModel = provider.GetRequiredService<LoginWindowViewModel>();
                     loginViewModel.InitializeAsync().GetAwaiter().GetResult();

                    var loginWindow = new LoginWindowView()
                    {
                        DataContext = loginViewModel
                    };

                    loginViewModel.OnLoginSucceeded += (sender, args) =>
                    {
                        var mainWindow = new MainWindow
                        {
                            DataContext = provider.GetRequiredService<MainWindowViewModel>(),
                        };
                        mainWindow.Show();
                       // loginWindow.Close();
                        desktop.MainWindow = mainWindow;
                    };

                    desktop.MainWindow = loginWindow;
              //      loginWindow.Show();
                }
                else
                {
                    desktop.MainWindow = new MainWindow
                    {
                        DataContext = provider.GetRequiredService<MainWindowViewModel>(),
                    };
                }
            }
            else
            {
                desktop.MainWindow = new ConfigurationErrorWindow();
            }
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}