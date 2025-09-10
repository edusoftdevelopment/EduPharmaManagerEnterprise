using POSApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace POSApp.ViewModels;

public partial class AppStateViewModel : ObservableObject
{
    [ObservableProperty]
    private AppUser? _user;

    public void SetAppUser(AppUser user)
    {
        User = user;
    }
}