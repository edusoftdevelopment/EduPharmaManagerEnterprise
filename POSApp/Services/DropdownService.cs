using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using POSApp.Helpers;

namespace POSApp.Services;

public class Dropdown<T>
{
    public required T Id { get; set; }
    public required string Label { get; set; }
}

public class DropdownService(
    DbHelper dbHelper,
    AppConfigService appConfigService,
    CacheService cacheService
)
{
    public Task<IEnumerable<Dropdown<string>>> GetDefaultDatabases()
    {
        const string query = "Select * from gen_SingleConnections Where ApplicationCodeName=@ApplicationCodeName";
        return dbHelper.ExecuteDefaultDbQueryAsync(query, row => new Dropdown<string>
        {
            Id = row.GetString(row.GetOrdinal("DefaultDB")),
            Label = row.GetString(row.GetOrdinal("Alias")),
        }, new SqlParameter
        {
            ParameterName = "@ApplicationCodeName",
            Value = appConfigService.ApplicationName
        });
    }

    public Task<IEnumerable<Dropdown<int>>> GetCollectedByList(
        int businessUnitId = 0)
    {
        string query =
            $"SELECT EmployeeCode, EmployeeName FROM gen_EmployeesInfo WHERE SalariedEmployee=1 AND Collecter=1 AND discontinue=0 {(businessUnitId != 0 ? " AND businessUnitID=@businessUnitID " : "")} ORDER BY EmployeeName";
        
        return dbHelper.ExecuteQueryAsync(query, row => new Dropdown<int>
        {
            Id = row.GetInt32(row.GetOrdinal("EmployeeCode")),
            Label = row.GetString(row.GetOrdinal("EmployeeName")),
        }, new SqlParameter
        {
            ParameterName = "@businessUnitID",
            Value = businessUnitId
        });
    }


    public Task<IEnumerable<Dropdown<int>>> GetBusinessUnitList(
        int defaultBusinessUnitId = 0)
    {
        var query = "SELECT BusinessUnitID, BusinessUnitTitle FROM gen_BusinessUnitInfo";

        if (defaultBusinessUnitId != 0)
        {
            query += $" WHERE BusinessUnitID = @DefaultBusinessUnitID";
        }

        return dbHelper.ExecuteQueryAsync(query, row => new Dropdown<int>
        {
            Id = row.GetByte(row.GetOrdinal("BusinessUnitID")),
            Label = row.GetString(row.GetOrdinal("BusinessUnitTitle")),
        }, new SqlParameter
        {
            ParameterName = "@DefaultBusinessUnitID",
            Value = defaultBusinessUnitId
        });
    }

    public Task<IEnumerable<Dropdown<int>>> GetSessionList()
    {
        const string query = " Select SessionID, 'TEST' SessionTitle  from SessionInfo Where currentSession = 1";
        return dbHelper.ExecuteQueryAsync(query, row => new Dropdown<int>
        {
            Id = row.GetByte(row.GetOrdinal("SessionID")),
            Label = row.GetString(row.GetOrdinal("SessionTitle")),
        });
    }

    public Task<IEnumerable<Dropdown<int>>> GetPartyList()
    {
        const string query = "Select PartyId, PartyName from gen_PartiesInfo Where Discontinue=0";
        return dbHelper.ExecuteQueryAsync(query, row => new Dropdown<int>
        {
            Id = row.GetInt16(row.GetOrdinal("PartyId")),
            Label = row.GetString(row.GetOrdinal("PartyName")),
        });
    }

    public async Task<IEnumerable<Dropdown<int>>> GetProductsList()
    {
        const string query =
            " SELECT ProductID, ProductTitle From vw_ProductsInfo WHERE Discontinue = 0 AND SalesStop = 0 AND PRODUCTID NOT IN(SELECT PRODUCTId FROM gen_ProductsInfo WHERE (gen_ProductsInfo.BrandID IN (SELECT ManufacturerID FROM data_StockTakingInfo WHERE Status = 'InProcess')) ) ORDER BY ProductTitle";

        var products = (await dbHelper.ExecuteQueryAsync(query, reader => new Dropdown<int>
        {
            Id = reader.GetInt32(reader.GetOrdinal("ProductID")),
            Label = reader.GetString(reader.GetOrdinal("ProductTitle")),
        })).ToList();

        cacheService.Set("products", products);

        return products;
    }

    public async Task<IEnumerable<Dropdown<int>>> GetStockHolderList()
    {
        const string query = "SELECT StockHolderID, HolderTitle FROM gen_StockHoldersInfo";
        return await dbHelper.ExecuteQueryAsync(query, row => new Dropdown<int>
        {
            Id = row.GetInt16(row.GetOrdinal("StockHolderID")),
            Label = row.GetString(row.GetOrdinal("HolderTitle")),
        });
    }
}