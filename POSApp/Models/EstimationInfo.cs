using System;
using System.Collections.Generic;

namespace POSApp.Models;

public class EstimationInfo
{
    public long EstimationID { get; set; }
    public long? EstimationNo { get; set; }
    public DateTime? EstimationDate { get; set; }
    public byte? SessionID { get; set; }
    public byte? BusinessUnitID { get; set; }
    public short? PartyID { get; set; }
    public string CustomerName { get; set; }
    public int? DoctorId { get; set; }
    public string DoctorName { get; set; }
    public string PatientName { get; set; }
    public DateTime? PrescriptionDate { get; set; }
    public bool? Gender { get; set; }
    public decimal? Age { get; set; }
    public byte? EstimationDays { get; set; }
    public string Symptoms { get; set; }
    public string Diagnose { get; set; }
    public decimal? GrossAmount { get; set; }
    public short? NewRecordByEmployeeID { get; set; }
    public DateTime? NewRecordDateTime { get; set; }
    public short? ModifyRecordByEmployeeID { get; set; }
    public DateTime? ModifyRecordDateTime { get; set; }
    public string EnterByEmployeeName { get; set; }
    public string ModifiedByEmployeeName { get; set; }
    public bool UnDeleteable { get; set; }
    public long? SaleReturnID { get; set; }
    public decimal? pMaxDiscount { get; set; }
    public string MobileNo { get; set; }
    public string BuyerName { get; set; }
    public int? CollectedById { get; set; }
    public bool PrescriptionReceived { get; set; }
    public IEnumerable<EstimationInfoDetail> Details { get; set; } = [];
}

public class EstimationInfoDetail
{
    public long EstimationMultiDetailID { get; set; } 
    public long EstimationID { get; set; } 
    public long ProductID { get; set; } 
    public int? StockHolderID { get; set; } 
    public string? BatchNo { get; set; } 
    public DateTime? ExpiryDate { get; set; } 
    public decimal? PackSize { get; set; } 
    public decimal? StripSize { get; set; } 
    public decimal? PacksQty { get; set; } 
    public decimal? UnitQty { get; set; } 
    public decimal? EstimationPacksQty { get; set; } 
    public decimal? EstimationUnitQty { get; set; } 
    public decimal? PackPrice { get; set; } 
    public string? ProductCode { get; set; } 
    public string? ProductTitle { get; set; } 
    public string? HolderTitle { get; set; } 
    public decimal? EstimationDays { get; set; } 
    public decimal? StockRate { get; set; } 
    public string? StockRateSource { get; set; } 
    public long? StockRateSourceID { get; set; } 
    public int? SupplierId { get; set; } 
    public bool SalesBase { get; set; } 
    public string? PartyName { get; set; } 
    public long BarCodeStockID { get; set; } 
    public decimal? MaxDiscount { get; set; } 
    public bool FixedQty { get; set; } 
    public decimal? PackPriceStock { get; set; } 
    public string? DuplicateProductName { get; set; } 
    public bool? TemperatureSensitive { get; set; } 
    public string? BinLocation { get; set; } 
    public bool ExcludeFromSales { get; set; }    
    
    public decimal Total => (UnitQty ?? 0) * (StockRate ?? 0);
}