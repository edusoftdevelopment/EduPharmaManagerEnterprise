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

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CurrentPage))]
    [NotifyPropertyChangedFor(nameof(IsEstimationActive))]
    [NotifyPropertyChangedFor(nameof(IsSalesInfoActive))]
    private ApplicationPageNames _currentPageKey = ApplicationPageNames.HomePage;

    public bool IsEstimationActive => CurrentPageKey == ApplicationPageNames.EstimationInfo;
    public bool IsSalesInfoActive => CurrentPageKey == ApplicationPageNames.SalesInfo;

    private readonly PageFactory _pageFactory;
    public PageViewModel CurrentPage => _pageFactory.GetPage(CurrentPageKey);

    
    public MainWindowViewModel()
    {
    }
    
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
                if (window.Name == "EstimationInfo")
                {
                    window.Activate();
                    return;
                }
            }
        }
        if (_pageFactory.GetPage(ApplicationPageNames.EstimationInfo) is EstimationInfoViewModel viewModel)
        {
            await viewModel.InitializeAsync();
            var window = new EstimationInfoWindow()
            {
                DataContext = viewModel
            };
            window.Show();
        }
    }
}