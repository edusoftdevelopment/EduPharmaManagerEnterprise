using Avalonia.Controls;
using POSApp.Models;
using POSApp.ViewModels;
using CommunityToolkit.Mvvm.Messaging;
using POSApp.Data;

namespace POSApp.Views;

public partial class EstimationInfoWindow : Window
{
    public EstimationInfoWindow()
    {
        InitializeComponent();
        
        if (Design.IsDesignMode)
            return;
        
        WeakReferenceMessenger.Default.Register<EstimationInfoWindow, SearchProductMessage>(this, static (w, m) =>
        {
            var dialog = new ProductSearchWindow
            {
                DataContext = new ProductSearchWindowViewModel()
            };
            m.Reply(dialog.ShowDialog<Product?>(w));
        });
    }
}