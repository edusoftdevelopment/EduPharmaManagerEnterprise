using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using POSApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using POSApp.Helpers;

namespace POSApp.ViewModels;

public partial class LoginWindowViewModel( ILoginService loginService, AppConfigService appConfigService, DropdownService dropdownService, AppStateViewModel appStateViewModel)
    : PageViewModel
{
    
    [ObservableProperty]
    private string _username = string.Empty;
    
    [ObservableProperty]
    private string _password = string.Empty;
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    private string _errorMessage = string.Empty;

    [ObservableProperty] private bool _isPending = false;
    
    [ObservableProperty]
    private Dropdown<string> _selectedDatabase;
    public event EventHandler? OnLoginSucceeded;
    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
    public IEnumerable<Dropdown<string>> DefaultDatabaseList { get; set; } = [];
    
    public async Task Initialize()
    {
        DefaultDatabaseList = (await dropdownService.GetDefaultDatabases()).ToList();
        if (DefaultDatabaseList.Any())
        {
            SelectedDatabase = DefaultDatabaseList.First();
        }
    }

    public async Task CheckConnection()
    {
        await loginService.CheckConnectionAsyc();
    }
    
    [RelayCommand]
    private async Task Login()
    {
        try
        {
            if (IsPending) return;
            
            IsPending = true;
            
            if (string.IsNullOrWhiteSpace(Username))
            {
                ErrorMessage = "Please enter a username";
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Please enter a password";
                return;
            }

            appConfigService.BuildAppConnectionString(SelectedDatabase.Id);

            var appUser = await loginService.Login(Username, Password);
            if (appUser != null)
            {
                appStateViewModel.SetAppUser(appUser);
                OnLoginSucceeded?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                ErrorMessage = "Invalid username or password";
            }
        }
        catch (Exception e)
        {
            ErrorMessage = e.Message;
        }
        finally
        {
            IsPending = false;
        }
    }
    
    [RelayCommand]
    private void ResetError()
    {
        ErrorMessage = string.Empty;
    }
}