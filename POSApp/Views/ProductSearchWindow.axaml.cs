using Avalonia.Controls;
using CommunityToolkit.Mvvm.Messaging;
using POSApp.Data;

namespace POSApp.Views;


public partial class ProductSearchWindow : Window
{
    public ProductSearchWindow()
    {
        InitializeComponent();

        WeakReferenceMessenger.Default.Register<ProductSearchWindow, ProductSearchDialogCloseMessage>(this,
            static (w, m) => { w.Close(m.SelectedProduct); });
    }
}