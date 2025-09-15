using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
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
    private readonly AppStateViewModel _appStateViewModel;
    private readonly EstimationInfoService _estimationInfoService;
    public IEnumerable<Dropdown<int>> CollectedByList { get; set; } = [];
    public IEnumerable<Dropdown<int>> SessionList { get; set; } = [];
    public IEnumerable<Dropdown<int>> BusinessUnitList { get; set; } = [];
    public IEnumerable<Dropdown<int>> PartyList { get; set; } = [];
    public IEnumerable<Dropdown<int>> CustomerList { get; set; } = [];
    public IEnumerable<Dropdown<int>> ProductList { get; set; } = [];
    public IEnumerable<Dropdown<int>> StockHolderList { get; set; } = [];

    #endregion

    #region Observables
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsViewMode))]
    [NotifyPropertyChangedFor(nameof(IsEditMode))]
    [NotifyPropertyChangedFor(nameof(IsNewMode))]
    private FormMode _formMode = FormMode.View;
    
    [ObservableProperty] private bool _isPrescriptionReceived;
    [ObservableProperty] private DateTime _selectedEntryDate = DateTime.Now;
    [ObservableProperty] private bool _isUnDeletable;
    [ObservableProperty] private long _estimationNo;
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
    [ObservableProperty] private byte _estimatedDays;
    [ObservableProperty] private string _doseOrDays;
    [ObservableProperty] private string _estimatedQty;
    [ObservableProperty] private string _price;
    [ObservableProperty] private string _estimatedGrossAmount;
    [ObservableProperty] private string _barCodeStock;
    [ObservableProperty] private string _productStock;
    [ObservableProperty] private string _binLocation;
    [ObservableProperty] private bool _wholeQtyLp;
    [ObservableProperty] private Dropdown<int> _selectedStockHolder;

    public ObservableCollection<EstimationInfoDetail> AddedDetail { get; set; } = [];

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
    
    
    // Derived State
    public bool IsViewMode => FormMode == FormMode.View;
    public bool IsNewMode => FormMode == FormMode.New;
    public bool IsEditMode => FormMode == FormMode.Edit;

    #endregion

    #region Constructors

    public EstimationInfoViewModel(
        EstimationInfoService estimationInfoService,
        DropdownService dropdownService,
        AppStateViewModel appStateViewModel
    )
    {
        PageName = ApplicationPageNames.EstimationInfo;
        _dropdownService = dropdownService;
        _appStateViewModel = appStateViewModel;
        _estimationInfoService = estimationInfoService;
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
        BusinessUnitList = await _dropdownService.GetBusinessUnitList(_appStateViewModel.User!.DefaultBusinessUnitID);
        PartyList = await _dropdownService.GetPartyList();
        CustomerList = await _dropdownService.GetCustomerList();
        ProductList = await _dropdownService.GetProductsList();
        StockHolderList = await _dropdownService.GetStockHolderList();
    }

    public async Task LoadRecordAsync()
    {
        var record = await _estimationInfoService.Load();

        if (record != null)
        {
            if (record.EstimationDate != null)
            {
                SelectedEntryDate = record.EstimationDate.Value;
            }   
            if (record.EstimationNo != null)
            {
                EstimationNo = record.EstimationNo.Value;
            }
            IsUnDeletable = record.UnDeleteable;
            
            if (record.SessionID != null)
            {
                var match = SessionList.FirstOrDefault(x => x.Id == record.SessionID.Value);
                if (match != null)
                {
                    SelectedSession = match;
                }
            }
            
            if (record.BusinessUnitID != null)
            {
                var match = BusinessUnitList.FirstOrDefault(x => x.Id == record.BusinessUnitID.Value);
                if (match != null)
                {
                    SelectedSession = match;
                }
            }
            
            if (record.PartyID != null)
            {
                var match = PartyList.FirstOrDefault(x => x.Id == record.PartyID.Value);
                if (match != null)
                {
                    SelectedParty = match;
                }
            }

            if (record.EstimationDays != null)
            {
                EstimatedDays = record.EstimationDays.Value;
            }
            
            AddedDetail = new ObservableCollection<EstimationInfoDetail>(record.Details);
        }
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