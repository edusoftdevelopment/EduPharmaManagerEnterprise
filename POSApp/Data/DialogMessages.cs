using CommunityToolkit.Mvvm.Messaging.Messages;
using POSApp.Models;

namespace POSApp.Data;

public class SearchProductMessage : AsyncRequestMessage<Product?>;

public class ProductSearchDialogCloseMessage(Product product)
{
    public Product SelectedProduct { get; } = product;
}