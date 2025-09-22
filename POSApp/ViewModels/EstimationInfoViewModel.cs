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
using POSApp.Helpers;

namespace POSApp.ViewModels;

public partial class EstimationInfoViewModel : PageViewModel
{
    #region Properties

    private const string MasterTableName = "data_EstimationInfo";
    private const string MasterIdField = "EstimationID";

    private readonly DropdownService _dropdownService;
    private readonly AppStateViewModel _appStateViewModel;
    private readonly EstimationInfoService _estimationInfoService;
    public IEnumerable<Dropdown<int>> CollectedByList { get; set; } = [];
    public IEnumerable<Dropdown<int>> SessionList { get; set; } = [];
    public IEnumerable<Dropdown<int>> BusinessUnitList { get; set; } = [];
    public IEnumerable<Dropdown<int>> PartyList { get; set; } = [];
    public IEnumerable<Dropdown<int>> ProductList { get; set; } = [];
    public IEnumerable<Dropdown<int>> StockHolderList { get; set; } = [];
    private string FormId { get; set; }

    #endregion

    #region Observables
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsViewMode))]
    [NotifyPropertyChangedFor(nameof(IsEditMode))]
    [NotifyPropertyChangedFor(nameof(IsNewMode))]
    private FormMode _formMode;
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(RecordCountString))]
    private long _recordCount;
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(RecordCountString))]
    private long _currentRecordNumber;
    
    [ObservableProperty] private long _estimationId;
    [ObservableProperty] private bool _isPrescriptionReceived;
    [ObservableProperty] private DateTime _selectedEntryDate = DateTime.Now;
    [ObservableProperty] private bool _isUnDeletable;
    [ObservableProperty] private long _estimationNo;

    [ObservableProperty] private int _selectedCollectedByID;
    [ObservableProperty] private int _selectedSessionID;
    [ObservableProperty] private int _selectedBusinessUnitID;
    [ObservableProperty] private int _selectedPartyID;
    [ObservableProperty] private string _customerName;
    [ObservableProperty] private string _salesReturnVNo;
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
    [ObservableProperty] private decimal _estimatedNetGrossAmount;
    [ObservableProperty] private string _lPValue;
    [ObservableProperty] private string _totalSalesReturnAmount;
    [ObservableProperty] private string _lMax;
    [ObservableProperty] private string _discountPercentage;
    [ObservableProperty] private string _estimatedDiscountAmount;
    [ObservableProperty] private string _estimatedTotalAmount;
    [ObservableProperty] private bool _isPrintStyleTwo;
    
    // Buttons
    [ObservableProperty] 
    private bool _isButtonFirstEnabled;
    [ObservableProperty] 
    private bool _isButtonPreviousEnabled;
    [ObservableProperty] 
    private bool _isButtonNextEnabled;
    [ObservableProperty] 
    private bool _isButtonLastEnabled;
    [ObservableProperty] 
    private bool _isButtonSaveEnabled;
    [ObservableProperty] 
    private bool _isButtonEditEnabled;
    [ObservableProperty] 
    private bool _isButtonDeleteEnabled;
    [ObservableProperty] 
    private bool _isButtonSearchEnabled;
    [ObservableProperty] 
    private bool _isButtonPrintEnabled;
    [ObservableProperty] 
    private bool _isButtonCancelEnabled;
    [ObservableProperty] 
    private bool _isButtonMaximizeEnabled;

    
    
    // Derived State
    public bool IsViewMode => FormMode == FormMode.View;
    public bool IsNewMode => FormMode == FormMode.New;
    public bool IsEditMode => FormMode == FormMode.Edit;

    public string RecordCountString => $"Record: {CurrentRecordNumber}/{RecordCount}";

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
        var collectedByTask = _dropdownService.GetCollectedByList(
            businessUnitId:_appStateViewModel.User!.DefaultBusinessUnitID);
        var sessionTask = _dropdownService.GetSessionList();
        var businessUnitTask = _dropdownService.GetBusinessUnitList(_appStateViewModel.User!.DefaultBusinessUnitID);
        var partyTask = _dropdownService.GetPartyList();
        var productTask = _dropdownService.GetProductsList();
        var stockHolderTask = _dropdownService.GetStockHolderList();
        var hostIdTask = _estimationInfoService.GetHostID();

        await Task.WhenAll(hostIdTask, collectedByTask, sessionTask, businessUnitTask, partyTask, productTask, stockHolderTask);
        
        CollectedByList = await collectedByTask;
        SessionList = await sessionTask;
        BusinessUnitList = await businessUnitTask;
        PartyList = await partyTask;
        ProductList = await productTask;
        StockHolderList = await stockHolderTask;

        FormId = $"{_appStateViewModel.User.LoginId}{await hostIdTask}{DateTime.Now:yyyyMMddHHmmss}";
    }

    public async Task LoadRecordAsync(MoveDirection moveDirection = MoveDirection.Last)
    {
        var masterInfo = await _estimationInfoService.Load(MasterTableName, MasterIdField, moveDirection, currentRecordId: EstimationId);
        
        RecordCount = masterInfo.RecordCount;
        CurrentRecordNumber = masterInfo.CurrentRecordNumber;
        
        var record = masterInfo.Data;
        
        EstimationId = record.EstimationID;
        if (record.EstimationDate != null)
        {
            SelectedEntryDate = record.EstimationDate.Value;
        }   
        if (record.EstimationNo != null)
        {
            EstimationNo = record.EstimationNo.Value;
        }

        IsPrescriptionReceived = record.PrescriptionReceived;
        
        IsUnDeletable = record.UnDeleteable;
        SelectedSessionID = record.SessionID ?? -1;
        SelectedBusinessUnitID = record.BusinessUnitID ?? -1;
        SelectedCollectedByID = record.CollectedById ?? -1;
        SelectedPartyID = record.PartyID ?? -1;
        CustomerName = record.CustomerName;
        EstimatedDays = record.EstimationDays ?? 0;
        EstimatedNetGrossAmount = record.GrossAmount ?? 0;
        
        AddedDetail = new ObservableCollection<EstimationInfoDetail>(record.Details);

        FormMode = FormMode.View;
    }

    public async Task SetupFormAsync()
    {
        // chk_AutoPrint.Value = GetSetting(App.EXEName, Me.Name, chk_AutoPrint.Name, 0)
        // chk_WithEstimationNo.Value = GetSetting(App.EXEName, Me.Name, chk_WithEstimationNo.Name, 0)
    }

    partial void OnFormModeChanged(FormMode value)
    {
        switch (value)
        {
            case FormMode.New:
                break;
            case FormMode.View:

                if (RecordCount > 0)
                {
                    if (RecordCount == 1)
                    {
                        IsButtonLastEnabled = false;
                        IsButtonPreviousEnabled = false;
                        IsButtonNextEnabled = false;
                        IsButtonSearchEnabled = false;
                        IsButtonFirstEnabled = false;
                    }
                    else
                    {
                        IsButtonLastEnabled = true;
                        IsButtonPreviousEnabled = true;
                        IsButtonNextEnabled = true;
                        IsButtonSearchEnabled = true;
                        IsButtonFirstEnabled = true;
                    }
                }
                else
                {
                    IsButtonFirstEnabled = false;
                    IsButtonLastEnabled = false;
                    IsButtonPreviousEnabled = false;
                    IsButtonNextEnabled = false;
                    IsButtonEditEnabled = false;
                    IsButtonDeleteEnabled = false;
                    IsButtonPrintEnabled = false;
                    // TODO: If pActualRecordCount = 0 Then pForm.cmd_Search.Enabled = False Else pForm.cmd_Search.Enabled = True
                }
                
                // TODO: If InStr(1, myRightsString, "ADD NEW") > 0 Then pForm.cmd_New.Enabled = True Else pForm.cmd_New.Enabled = False
                
                
                break;
            case FormMode.Edit:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ChangeControlMode(FormMode mode)
    {
        // TODO: complete this
        switch (mode)
        {
            case FormMode.New:
                break;
            case FormMode.View:
                break;
            case FormMode.Edit:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
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
    private async Task First()
    {
        await LoadRecordAsync(MoveDirection.First);
    }

    [RelayCommand]
    private async Task Previous()
    {
        try
        {
            await LoadRecordAsync(MoveDirection.Previous);
        }
        catch (Exception ex)
        {
            if (ex is NoPreviousRecordException)
            {
                
            }
        }
    }

    [RelayCommand]
    private async Task Next()
    {
        try
        {
            await LoadRecordAsync(MoveDirection.Next);
        }
        catch (Exception ex)
        {
            if (ex is NoNextRecordException)
            {
                
            }
        }
    }
    [RelayCommand]
    private async Task Last()
    {
        await LoadRecordAsync(MoveDirection.Last);
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