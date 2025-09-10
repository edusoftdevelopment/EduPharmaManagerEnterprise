using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using POSApp.Data;
using POSApp.Models;
using POSApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace POSApp.ViewModels;


public partial class EstimationInfoViewModel : PageViewModel
{
    #region Properties
    private readonly DropdownService _dropdownService;
    public IEnumerable<Dropdown<int>> CollectedByList { get; set; } = [];
    public IEnumerable<Dropdown<int>> SessionList { get; set; } = [];
    public IEnumerable<Dropdown<int>> BusinessUnitList { get; set; } = [];
    public IEnumerable<Dropdown<int>> PartyList { get; set; } = [];
    public IEnumerable<Dropdown<int>> CustomerList { get; set; } = [];
    public IEnumerable<Dropdown<int>> ProductList { get; set; } = [];
    public IEnumerable<Dropdown<int>> StockHolderList { get; set; } = [];
    

    #endregion

    #region Observables

    [ObservableProperty] private bool _isPrescriptionReceived;
    [ObservableProperty] private DateTime _selectedEntryDate = DateTime.Now;
    [ObservableProperty] private bool _isUnDeletable;
    [ObservableProperty] private string _estimationNo;
    [ObservableProperty] private Dropdown<int> _selectedCollectedBy;
    [ObservableProperty] private Dropdown<int> _selectedSession;
    [ObservableProperty] private Dropdown<int> _selectedBusinessUnit;
    [ObservableProperty] private string _salesReturnVNo;
    [ObservableProperty] private Dropdown<int> _selectedParty;
    [ObservableProperty] private Dropdown<int> _selectedCustomer;
    [ObservableProperty] private string _salesReturnAmount;

    // Header
    [ObservableProperty] private string _productCode;
    [ObservableProperty] private Dropdown<int> _selectedProduct;
    [ObservableProperty] private string _batchNo;
    [ObservableProperty] private string _expiryDate;
    [ObservableProperty] private string _size;
    [ObservableProperty] private string _pSize;
    [ObservableProperty] private string _estimatedDays;
    [ObservableProperty] private string _doseOrDays;
    [ObservableProperty] private string _estimatedQty;
    [ObservableProperty] private string _price;
    [ObservableProperty] private string _estimatedGrossAmount;
    [ObservableProperty] private string _barCodeStock;
    [ObservableProperty] private string _productStock;
    [ObservableProperty] private string _binLocation;
    [ObservableProperty] private bool _wholeQtyLp;
    [ObservableProperty] private Dropdown<int> _selectedStockHolder;
    
    public ObservableCollection<Product> AddedProducts { get; } = [];

    // Footer
    [ObservableProperty] private bool _isAutoPrintEstimation;
    [ObservableProperty] private bool _isAutoCustomerToken;
    [ObservableProperty] private bool _isAutoPrintF7;
    [ObservableProperty] private string _estimatedDaysFooter;
    [ObservableProperty] private string _items;
    [ObservableProperty] private string _itemsQty;
    [ObservableProperty] private string _estimatedNetGrossAmount;
    [ObservableProperty] private string _lPValue;
    [ObservableProperty] private string _totalSalesReturnAmount;
    [ObservableProperty] private string _lMax;
    [ObservableProperty] private string _discountPercentage;
    [ObservableProperty] private string _estimatedDiscountAmount;
    [ObservableProperty] private string _estimatedTotalAmount;
    [ObservableProperty] private bool _isPrintStyleTwo;

    #endregion

    #region Constructors
    public EstimationInfoViewModel(DropdownService dropdownService)
    {
        PageName = ApplicationPageNames.EstimationInfo;
        _dropdownService = dropdownService;
        
    }
    #endregion

    #region Methods

    public async Task InitializeAsync()
    {
        await LoadDropdownAsync();
    }

    private async Task LoadDropdownAsync()
    {
        CollectedByList = await _dropdownService.GetCollectedByList();
        SessionList = await _dropdownService.GetSessionList();
        BusinessUnitList = await _dropdownService.GetBusinessUnitList();
        PartyList = await _dropdownService.GetPartyList();
        CustomerList = await _dropdownService.GetCustomerList();
        ProductList = await _dropdownService.GetProductsList();
        StockHolderList = await _dropdownService.GetStockHolderList();
    }

    #endregion
    
    #region Commands

    [RelayCommand]
    private void HideGridColumns()
    {
    }

    [RelayCommand]
    private void EstimationNoGo()
    {
    }

    [RelayCommand]
    private void MarketDemand()
    {
    }

    [RelayCommand]
    private void AddRow()
    {
        AddedProducts.Add(new Product {ProductId  = SelectedProduct.Id, ProductCode = ProductCode, ProductTitle = SelectedProduct.Label, ProductType = "Type"});
    }

    [RelayCommand]
    private void ClearRow()
    {
    }

    [RelayCommand]
    private void First()
    {
    }

    [RelayCommand]
    private void Previous()
    {
    }

    [RelayCommand]
    private void Next()
    {
    }

    [RelayCommand]
    private void Last()
    {
    }

    [RelayCommand]
    private void SaveAsync()
    {
        var save = new
        {
            
        };
    }

    [RelayCommand]
    private void Edit()
    {
    }

    [RelayCommand]
    private void Delete()
    {
    }

    [RelayCommand]
    private void Search()
    {
    }

    [RelayCommand]
    private void Print()
    {
    }

    [RelayCommand]
    private void Cancel()
    {
    }

    [RelayCommand]
    private void Maximize()
    {
    }

    [RelayCommand]
    private async Task ProductSearchDialogAsync()
    {
        var product = await WeakReferenceMessenger.Default.Send(new SearchProductMessage());
        Debug.WriteLine(product);
    }
    #endregion
}