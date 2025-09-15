using System.Diagnostics;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Notifications;
using POSApp.Data;
using POSApp.Factories;
using POSApp.Helpers;
using POSApp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace POSApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private bool _isSidebarOpen = true;

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(IsEstimationActive))]
    private ApplicationPageNames _currentPageKey = ApplicationPageNames.EstimationInfo;

    public bool IsEstimationActive => CurrentPageKey == ApplicationPageNames.EstimationInfo;

    private readonly PageFactory _pageFactory;

    public MainWindowViewModel(PageFactory pageFactory)
    {
        _pageFactory = pageFactory;
    }


    [RelayCommand]
    private void ToggleSidebar()
    {
        IsSidebarOpen = !IsSidebarOpen;
    }

    [RelayCommand]
    private void GoToPage(string pageKey)
    {
        if (pageKey == "estimation")
        {
        }
    }

    [RelayCommand]
    private async Task GoToEstimationInfo()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            foreach (var window in desktop.Windows)
            {
                if (window.Tag != null)
                {
                    if ((string)window.Tag == "EstimationInfoKey")
                    {
                        window.Activate();
                        return;
                    }
                }
            }
        }

        if (_pageFactory.GetPage(ApplicationPageNames.LoadingSplash) is LoadingSplashWindowViewModel viewModel)
        {
            var window = new LoadingSplashWindow
            {
                DataContext = viewModel
            };
            window.Show();

            var result = await viewModel.ShowEstimationInfoPage();
            if (result)
            {
                Debug.WriteLine("Splash Window Closed...");

                window.Close();
            }
        }

        // if (_pageFactory.GetPage(ApplicationPageNames.EstimationInfo) is EstimationInfoViewModel viewModel)
        // {
        //     await viewModel.InitializeAsync();
        //     var window = new EstimationInfoWindow()
        //     {
        //         DataContext = viewModel
        //     };
        //     window.Show();
        // }
    }
}