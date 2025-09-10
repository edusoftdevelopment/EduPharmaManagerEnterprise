using POSApp.Data;
using CommunityToolkit.Mvvm.ComponentModel;

namespace POSApp.ViewModels;

public partial class PageViewModel : ViewModelBase
{
    [ObservableProperty]
    private ApplicationPageNames _pageName;
}