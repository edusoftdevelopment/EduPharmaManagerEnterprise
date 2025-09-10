using POSApp.Models;
using POSApp.Views;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using POSApp.Data;

namespace POSApp.ViewModels;

public partial class ProductSearchWindowViewModel : ViewModelBase
{
    [RelayCommand]
    private void SearchProduct()
    {
        WeakReferenceMessenger.Default.Send(new ProductSearchDialogCloseMessage(new Product()
        {
            ProductCode = "lo",
            ProductId = 1,
            ProductTitle = "lol",
            ProductType = "lol"
        }));       
    }
}