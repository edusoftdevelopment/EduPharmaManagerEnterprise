using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using POSApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace POSApp.ViewModels;

public partial class LoginWindowViewModel(ILoginService loginService, DropdownService dropdownService, AppStateViewModel appStateViewModel)
    : PageViewModel
{
    
    [ObservableProperty]
    private string _username = string.Empty;
    
    [ObservableProperty]
    private string _password = string.Empty;
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    private string _errorMessage = string.Empty;
    
    [ObservableProperty]
    private Dropdown<string> _selectedDatabase;
    
    public event EventHandler? OnLoginSucceeded;
    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
    public IEnumerable<Dropdown<string>> DefaultDatabaseList { get; set; } = [];
    
    public async Task InitializeAsync()
    {
        DefaultDatabaseList = (await dropdownService.GetDefaultDatabases()).ToList();
        if (DefaultDatabaseList.Any())
        {
            SelectedDatabase = DefaultDatabaseList.First();
        }
    }
    
    [RelayCommand]
    private async Task LoginAsync()
    {
        try
        {
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

            var appUser = await loginService.Login(Username, Password);
            appStateViewModel.SetAppUser(appUser);
            OnLoginSucceeded?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception e)
        {
            ErrorMessage = e.Message;
        }
    }
    
    [RelayCommand]
    private void ResetError()
    {
        ErrorMessage = string.Empty;
    }
}