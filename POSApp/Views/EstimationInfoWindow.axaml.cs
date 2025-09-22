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
            if (!w.IsActive) return;
            
            var vm = w.DataContext as EstimationInfoViewModel;
            var dialog = new ProductSearchWindow
            {
                DataContext = new ProductSearchWindowViewModel(vm?.ProductList ?? [])
            };
            m.Reply(dialog.ShowDialog<Product?>(w));
        });
        
        Closed += (_, _) => WeakReferenceMessenger.Default.Unregister<SearchProductMessage>(this);
    }
}