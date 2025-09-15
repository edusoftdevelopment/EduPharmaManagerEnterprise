using System.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;
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
        _ = InitializeUiAsync();
        base.OnFrameworkInitializationCompleted();
    }

    private async Task InitializeUiAsync()
    {
        var services = new ServiceCollection();

        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<AppStateViewModel>();
        services.AddSingleton<AppConfigService>();
        services.AddSingleton<DbHelper>();
        services.AddSingleton<CacheService>();

        services.AddScoped<DropdownService>();
        services.AddScoped<ILoginService, LoginService>();
        services.AddScoped<EstimationInfoService>();

        services.AddTransient<LoginWindowViewModel>();
        services.AddTransient<EstimationInfoViewModel>();
        services.AddTransient<LoadingSplashWindowViewModel>();

        services.AddSingleton<Func<ApplicationPageNames, PageViewModel>>(x => name => name switch
        {
            ApplicationPageNames.EstimationInfo => x.GetRequiredService<EstimationInfoViewModel>(),
            ApplicationPageNames.LoadingSplash => x.GetRequiredService<LoadingSplashWindowViewModel>(),
            _ => throw new ArgumentOutOfRangeException(nameof(name), name, null)
        });
        services.AddSingleton<PageFactory>();

        var provider = services.BuildServiceProvider();

        var configService = provider.GetRequiredService<AppConfigService>();
        var hasConfigurationFile = configService.Load();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            DisableAvaloniaDataAnnotationValidation();

            if (hasConfigurationFile)
            {
                var appStateViewModel = provider.GetRequiredService<AppStateViewModel>();
                if (appStateViewModel.User is null)
                {
                    var loadingSplashWindowViewModel = provider.GetRequiredService<LoadingSplashWindowViewModel>();
                    var loadingSplashWindow = new LoadingSplashWindow
                    {
                        DataContext = loadingSplashWindowViewModel
                    };
                    loadingSplashWindowViewModel.LoadingText = "Checking Database Connection...";
                    loadingSplashWindow.Show();

                    try
                    {
                        var loginViewModel = provider.GetRequiredService<LoginWindowViewModel>();
                        
                        await loginViewModel.CheckConnection();

                        loadingSplashWindowViewModel.LoadingText = "Preparing Login Page...";
                        await loginViewModel.Initialize();

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
                            loginWindow.Close();
                            desktop.MainWindow = mainWindow;
                        };

                        desktop.MainWindow = loginWindow;
                        loginWindow.Show();
                        loadingSplashWindow.Close();
                    }
                    catch (Exception e)
                    {
                        loadingSplashWindowViewModel.ErrorText = e.ToString();
                        desktop.MainWindow = loadingSplashWindow;
                    }
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