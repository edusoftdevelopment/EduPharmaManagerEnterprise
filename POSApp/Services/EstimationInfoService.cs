using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using POSApp.Extensions;
using POSApp.Helpers;
using POSApp.Models;

namespace POSApp.Services;

public class EstimationInfoService(
    DbHelper db
)
{
    public Task<IEnumerable<EstimationInfo>> LoadAll()
    {
        string query =
            "SELECT data_EstimationInfo.EstimationID, data_EstimationInfo.EstimationNo, data_EstimationInfo.EstimationDate, data_EstimationInfo.SessionID, data_EstimationInfo.BusinessUnitID, data_EstimationInfo.PartyID, data_EstimationInfo.CustomerName, data_EstimationInfo.DoctorId, data_EstimationInfo.DoctorName, data_EstimationInfo.PatientName, data_EstimationInfo.PrescriptionDate, data_EstimationInfo.Gender, data_EstimationInfo.Age, data_EstimationInfo.EstimationDays, data_EstimationInfo.Symptoms, data_EstimationInfo.Diagnose, data_EstimationInfo.GrossAmount, data_EstimationInfo.NewRecordByEmployeeID, data_EstimationInfo.NewRecordDateTime, data_EstimationInfo.ModifyRecordByEmployeeID, data_EstimationInfo.ModifyRecordDateTime, Employees.EmployeeName AS EnterByEmployeeName, Employees_1.EmployeeName AS ModifiedByEmployeeName, data_EstimationInfo.UnDeleteable, data_EstimationInfo.SaleReturnID, pMaxDiscount, data_EstimationInfo.MobileNo, data_EstimationInfo.BuyerName, data_EstimationInfo.CollectedById, data_EstimationInfo.PrescriptionReceived FROM data_EstimationInfo LEFT OUTER JOIN Employees ON data_EstimationInfo.NewRecordByEmployeeID = Employees.EmployeeCode LEFT OUTER JOIN Employees Employees_1 ON data_EstimationInfo.ModifyRecordByEmployeeID = Employees_1.EmployeeCode WHERE (data_EstimationInfo.EstimationID > 0) AND (data_EstimationInfo.Saved = 1)";

        return db.ExecuteQueryAsync(query, reader => new EstimationInfo
            {
                EstimationID = reader.GetInt64(reader.GetOrdinal("EstimationID")),
                EstimationNo = reader.GetNullable<long>("EstimationNo"),
                EstimationDate = reader.GetNullable<DateTime>("EstimationDate"),
                SessionID = reader.GetNullable<byte>("SessionID"),
                BusinessUnitID = reader.GetNullable<byte>("BusinessUnitID"),
                PartyID = reader.GetNullable<short>("PartyID"),
                CustomerName = reader["CustomerName"] as string,
                DoctorId = reader.GetNullable<int>("DoctorId"),
                DoctorName = reader["DoctorName"] as string,
                PatientName = reader["PatientName"] as string,
                PrescriptionDate = reader.GetNullable<DateTime>("PrescriptionDate"),
                Gender = reader.GetNullable<bool>("Gender"),
                Age = reader.GetNullable<decimal>("Age"),
                EstimationDays = reader.GetNullable<byte>("EstimationDays"),
                Symptoms = reader["Symptoms"] as string,
                Diagnose = reader["Diagnose"] as string,
                GrossAmount = reader.GetNullable<decimal>("GrossAmount"),
                NewRecordByEmployeeID = reader.GetNullable<short>("NewRecordByEmployeeID"),
                NewRecordDateTime = reader.GetNullable<DateTime>("NewRecordDateTime"),
                ModifyRecordByEmployeeID = reader.GetNullable<short>("ModifyRecordByEmployeeID"),
                ModifyRecordDateTime = reader.GetNullable<DateTime>("ModifyRecordDateTime"),
                EnterByEmployeeName = reader["EnterByEmployeeName"] as string,
                ModifiedByEmployeeName = reader["ModifiedByEmployeeName"] as string,
                UnDeleteable = reader.GetBoolean(reader.GetOrdinal("UnDeleteable")),
                SaleReturnID = reader.GetNullable<long>("SaleReturnID"),
                pMaxDiscount = reader.GetNullable<decimal>("pMaxDiscount"),
                MobileNo = reader["MobileNo"] as string,
                BuyerName = reader["BuyerName"] as string,
                CollectedById = reader.GetNullable<int>("CollectedById"),
                PrescriptionReceived = reader.GetBoolean(reader.GetOrdinal("PrescriptionReceived"))
            }
        );
    }


    public async Task<EstimationInfo?> Load()
    {
        string query =
            "SELECT data_EstimationInfo.EstimationID, data_EstimationInfo.EstimationNo, data_EstimationInfo.EstimationDate, data_EstimationInfo.SessionID, data_EstimationInfo.BusinessUnitID, data_EstimationInfo.PartyID, data_EstimationInfo.CustomerName, data_EstimationInfo.DoctorId, data_EstimationInfo.DoctorName, data_EstimationInfo.PatientName, data_EstimationInfo.PrescriptionDate, data_EstimationInfo.Gender, data_EstimationInfo.Age, data_EstimationInfo.EstimationDays, data_EstimationInfo.Symptoms, data_EstimationInfo.Diagnose, data_EstimationInfo.GrossAmount, data_EstimationInfo.NewRecordByEmployeeID, data_EstimationInfo.NewRecordDateTime, data_EstimationInfo.ModifyRecordByEmployeeID, data_EstimationInfo.ModifyRecordDateTime, Employees.EmployeeName AS EnterByEmployeeName, Employees_1.EmployeeName AS ModifiedByEmployeeName, data_EstimationInfo.UnDeleteable, data_EstimationInfo.SaleReturnID, pMaxDiscount, data_EstimationInfo.MobileNo, data_EstimationInfo.BuyerName, data_EstimationInfo.CollectedById, data_EstimationInfo.PrescriptionReceived FROM data_EstimationInfo LEFT OUTER JOIN Employees ON data_EstimationInfo.NewRecordByEmployeeID = Employees.EmployeeCode LEFT OUTER JOIN Employees Employees_1 ON data_EstimationInfo.ModifyRecordByEmployeeID = Employees_1.EmployeeCode WHERE (data_EstimationInfo.EstimationID > 0) AND (data_EstimationInfo.Saved = 1)";

        var records = (await db.ExecuteQueryAsync(query, reader => new EstimationInfo
            {
                EstimationID = reader.GetInt64(reader.GetOrdinal("EstimationID")),
                EstimationNo = reader.GetNullable<long>("EstimationNo"),
                EstimationDate = reader.GetNullable<DateTime>("EstimationDate"),
                SessionID = reader.GetNullable<byte>("SessionID"),
                BusinessUnitID = reader.GetNullable<byte>("BusinessUnitID"),
                PartyID = reader.GetNullable<short>("PartyID"),
                CustomerName = reader["CustomerName"] as string,
                DoctorId = reader.GetNullable<int>("DoctorId"),
                DoctorName = reader["DoctorName"] as string,
                PatientName = reader["PatientName"] as string,
                PrescriptionDate = reader.GetNullable<DateTime>("PrescriptionDate"),
                Gender = reader.GetNullable<bool>("Gender"),
                Age = reader.GetNullable<decimal>("Age"),
                EstimationDays = reader.GetNullable<byte>("EstimationDays"),
                Symptoms = reader["Symptoms"] as string,
                Diagnose = reader["Diagnose"] as string,
                GrossAmount = reader.GetNullable<decimal>("GrossAmount"),
                NewRecordByEmployeeID = reader.GetNullable<short>("NewRecordByEmployeeID"),
                NewRecordDateTime = reader.GetNullable<DateTime>("NewRecordDateTime"),
                ModifyRecordByEmployeeID = reader.GetNullable<short>("ModifyRecordByEmployeeID"),
                ModifyRecordDateTime = reader.GetNullable<DateTime>("ModifyRecordDateTime"),
                EnterByEmployeeName = reader["EnterByEmployeeName"] as string,
                ModifiedByEmployeeName = reader["ModifiedByEmployeeName"] as string,
                UnDeleteable = reader.GetBoolean(reader.GetOrdinal("UnDeleteable")),
                SaleReturnID = reader.GetNullable<long>("SaleReturnID"),
                pMaxDiscount = reader.GetNullable<decimal>("pMaxDiscount"),
                MobileNo = reader["MobileNo"] as string,
                BuyerName = reader["BuyerName"] as string,
                CollectedById = reader.GetNullable<int>("CollectedById"),
                PrescriptionReceived = reader.GetBoolean(reader.GetOrdinal("PrescriptionReceived"))
            }
        )).ToList();

        if (records.Count > 0)
        {
            var record = records.First();
            var detail = await LoadDetail(record.EstimationID);
            record.Details = detail;
            return record;
        }

        return null;
    }

    private Task<IEnumerable<EstimationInfoDetail>> LoadDetail(long EstimationID)
    {
        const string query =
            "SELECT data_EstimationMultiDetail.EstimationMultiDetailID, data_EstimationMultiDetail.EstimationID, data_EstimationMultiDetail.ProductID, data_EstimationMultiDetail.StockHolderID, data_EstimationMultiDetail.BatchNo, data_EstimationMultiDetail.ExpiryDate, data_EstimationMultiDetail.PackSize, data_EstimationMultiDetail.StripSize, data_EstimationMultiDetail.PacksQty, data_EstimationMultiDetail.UnitQty, data_EstimationMultiDetail.EstimationPacksQty, data_EstimationMultiDetail.EstimationUnitQty, data_EstimationMultiDetail.PackPrice, vw_ProductsInfo.ProductCode, vw_ProductsInfo.ProductTitle, gen_StockHoldersInfo.HolderTitle, data_EstimationMultiDetail.EstimationDays, data_EstimationMultiDetail.StockRate, data_EstimationMultiDetail.StockRateSource, data_EstimationMultiDetail.StockRateSourceID, data_EstimationMultiDetail.SupplierId, data_EstimationMultiDetail.SalesBase, gen_PartiesInfo.PartyName, ISNULL(data_EstimationMultiDetail.BarCodeStockID, 0) BarCodeStockID, vw_ProductsInfo.MaxDiscount, data_EstimationMultiDetail.FixedQty, data_EstimationMultiDetail.PackPriceStock, vw_ProductsInfo.DuplicateProductName, vw_ProductsInfo.TemperatureSensitive, vw_ProductsInfo.BinLocation, data_EstimationMultiDetail.ExcludeFromSales FROM data_EstimationMultiDetail LEFT OUTER JOIN gen_PartiesInfo ON data_EstimationMultiDetail.SupplierID = gen_PartiesInfo.PartyId LEFT OUTER JOIN gen_StockHoldersInfo ON data_EstimationMultiDetail.StockHolderID = gen_StockHoldersInfo.StockHolderID LEFT OUTER JOIN vw_ProductsInfo ON data_EstimationMultiDetail.ProductID = vw_ProductsInfo.ProductID WHERE (data_EstimationMultiDetail.EstimationID = @EstimationID) ORDER BY vw_ProductsInfo.ProductTitle";

        return db.ExecuteQueryAsync(query, reader => new EstimationInfoDetail()
            {
                BarCodeStockID = reader.GetInt64(reader.GetOrdinal("BarCodeStockID")),
                BatchNo = reader["BatchNo"] as string,
                BinLocation = reader["BinLocation"] as string,
                DuplicateProductName = reader["DuplicateProductName"] as string,
                EstimationDays = reader.GetNullable<decimal>("EstimationDays"),
                EstimationID = reader.GetInt64(reader.GetOrdinal("EstimationID")),
                EstimationMultiDetailID = reader.GetInt64(reader.GetOrdinal("EstimationMultiDetailID")),
                EstimationPacksQty = reader.GetNullable<decimal>("EstimationPacksQty"),
                EstimationUnitQty = reader.GetNullable<decimal>("EstimationUnitQty"),
                ExcludeFromSales = reader.GetBoolean(reader.GetOrdinal("ExcludeFromSales")),
                ExpiryDate = reader.GetNullable<DateTime>("ExpiryDate"),
                PackSize = reader.GetNullable<decimal>("PackSize"),
                FixedQty = reader.GetBoolean(reader.GetOrdinal("FixedQty")),
                HolderTitle = reader["HolderTitle"] as string,
                MaxDiscount = reader.GetNullable<decimal>("MaxDiscount"),
                PackPrice = reader.GetNullable<decimal>("PackPrice"),
                PackPriceStock = reader.GetNullable<decimal>("PackPriceStock"),
                PacksQty = reader.GetNullable<decimal>("PacksQty"),
                UnitQty = reader.GetNullable<decimal>("UnitQty"),
                PartyName = reader["PartyName"] as string,
                ProductCode = reader["ProductCode"] as string,
                ProductTitle = reader["ProductTitle"] as string,
                ProductID = reader.GetInt64(reader.GetOrdinal("ProductID")),
                SalesBase = reader.GetBoolean(reader.GetOrdinal("SalesBase")),
                StockHolderID = reader.GetInt16(reader.GetOrdinal("StockHolderID")),
                StockRate = reader.GetNullable<decimal>("StockRate"),
                StockRateSource = reader["StockRateSource"] as string,
                StockRateSourceID = reader.GetInt64(reader.GetOrdinal("StockRateSourceID")),
                StripSize = reader.GetNullable<decimal>("StripSize"),
                SupplierId = reader.GetInt32(reader.GetOrdinal("SupplierID")),
                TemperatureSensitive = reader.GetNullable<bool>("TemperatureSensitive"),
            },
            new SqlParameter { ParameterName = "@EstimationID", Value = EstimationID });
    }
}

/*
             DataTable schemaTable = reader.GetSchemaTable();

           if (schemaTable != null)
           {
               for (int i = 0; i < schemaTable.Rows.Count; i++)
               {
                   var column = schemaTable.Rows[i];
                   string columnName = column["ColumnName"]?.ToString() ?? $"Col{i}";
                   bool allowsNull = column["AllowDBNull"] is bool b && b;
                   string dataType = column["DataType"] is Type t ? t.Name : "Unknown";

Debug.WriteLine($"public {dataType}{(allowsNull ? "?" : "")} {columnName} {{ get; set; }}  // AllowsNull: {allowsNull}");

                   Debug.WriteLine($"[{i}] {columnName} ({dataType}) - Allows NULL: {allowsNull}");
               }
           }

 */