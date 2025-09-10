using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace POSApp.Views;

public partial class LoginWindowView : Window
{
    public LoginWindowView()
    {
        InitializeComponent();

        TbUsername.AttachedToVisualTree += (sender, args) => TbUsername.Focus();
    }
}