using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using POSApp.Data;
using POSApp.Factories;
using POSApp.Views;

namespace POSApp.ViewModels;

public partial class LoadingSplashWindowViewModel : PageViewModel
{

    private readonly PageFactory _pageFactory;

    [ObservableProperty] private string _loadingText;
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    [NotifyPropertyChangedFor(nameof(SystemDecoration))]
    private string _errorText;

    public LoadingSplashWindowViewModel(PageFactory pageFactory)
    {
        PageName = ApplicationPageNames.LoadingSplash;
        _pageFactory = pageFactory;
    }
    
    public bool HasError => !string.IsNullOrEmpty(ErrorText);
    public string SystemDecoration => !string.IsNullOrEmpty(ErrorText) ? "Full" : "None";
    
    public async Task<bool> ShowEstimationInfoPage()
    {
        try
        {
            LoadingText = "Loading dropdowns...";
            ErrorText = "";
            if (_pageFactory.GetPage(ApplicationPageNames.EstimationInfo) is not EstimationInfoViewModel estimationInfoViewModel) return false;
            
            await estimationInfoViewModel.InitializeAsync();
            
            LoadingText = "Loading record data...";
            
            await estimationInfoViewModel.LoadRecordAsync();
            
            LoadingText = "Setting up form...";
            
            await estimationInfoViewModel.SetupFormAsync();

            
            var window = new EstimationInfoWindow()
            {
                DataContext = estimationInfoViewModel
            };
            window.Show();
            
            return true;
        }
        catch (Exception ex)
        {
            ErrorText = "Error: " + ex.ToString(); // TODO: change message before publish
            return false;
        }
    }
}