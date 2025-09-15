using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using POSApp.Models;
using POSApp.Views;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using POSApp.Data;
using POSApp.Services;

namespace POSApp.ViewModels;

public partial class ProductSearchWindowViewModel(IEnumerable<Dropdown<int>> productList) : ViewModelBase
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FilteredProductList))]
    private string _searchQuery = "";
    
    [ObservableProperty]
    private Dropdown<int> _selectedProduct;

    public IEnumerable<Dropdown<int>>? FilteredProductList =>
        string.IsNullOrEmpty(SearchQuery)
            ? productList
            : productList.Where(p => p.Label.Contains(SearchQuery, System.StringComparison.CurrentCultureIgnoreCase));

    partial void OnSelectedProductChanged(Dropdown<int> value)
    {
        WeakReferenceMessenger.Default.Send(new ProductSearchDialogCloseMessage(new Product()
        {
            ProductCode = "",
            ProductId = value.Id,
            ProductTitle = value.Label,
            ProductType = ""
        }));
    }
    
}