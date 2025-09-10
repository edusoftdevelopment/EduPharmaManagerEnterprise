using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using POSApp.Helpers;

namespace POSApp.Services;

public class Dropdown<T>
{
    public required T Id { get; set; }
    public required string Label { get; set; }
}

public class DropdownService(DbHelper dbHelper, AppConfigService appConfigService)
{
    public async Task<IEnumerable<Dropdown<string>>> GetDefaultDatabases()
    {
        const string query = "Select * from gen_SingleConnections Where ApplicationCodeName=@ApplicationCodeName";
        return await dbHelper.ExecuteQueryAsync(query, row => new Dropdown<string>
        {
            Id = row.GetString(row.GetOrdinal("Id")),
            Label = row.GetString(row.GetOrdinal("Label")),
        }, new SqlParameter
        {
            ParameterName = "@ApplicationCodeName",
            Value = appConfigService.ApplicationName
        });
    }

    public Task<IEnumerable<Dropdown<int>>> GetCollectedByList()
    {
        return Task.FromResult<IEnumerable<Dropdown<int>>>(new List<Dropdown<int>>
        {
            new() { Id = 1, Label = "Edusoft" },
            new() { Id = 2, Label = "Paragmtic" },
            new() { Id = 3, Label = "Techno Phile" }
        });
    }


    public Task<IEnumerable<Dropdown<int>>> GetBusinessUnitList()
    {
        return Task.FromResult<IEnumerable<Dropdown<int>>>(new List<Dropdown<int>>
        {
            new() { Id = 1, Label = "Edusoft" },
            new() { Id = 2, Label = "Paragmtic" },
            new() { Id = 3, Label = "Techno Phile" }
        });
    }

    public Task<IEnumerable<Dropdown<int>>> GetSessionList()
    {
        return Task.FromResult<IEnumerable<Dropdown<int>>>(new List<Dropdown<int>>
        {
            new() { Id = 1, Label = "Spring 2023" },
            new() { Id = 2, Label = "Fall 2023" },
            new() { Id = 3, Label = "Winter 2024" }
        });
    }

    public Task<IEnumerable<Dropdown<int>>> GetPartyList()
    {
        return Task.FromResult<IEnumerable<Dropdown<int>>>(new List<Dropdown<int>>
        {
            new() { Id = 1, Label = "Party A" },
            new() { Id = 2, Label = "Party B" },
            new() { Id = 3, Label = "Party C" }
        });
    }

    public Task<IEnumerable<Dropdown<int>>> GetCustomerList()
    {
        return Task.FromResult<IEnumerable<Dropdown<int>>>(new List<Dropdown<int>>
        {
            new() { Id = 1, Label = "Alice" },
            new() { Id = 2, Label = "Bob" },
            new() { Id = 3, Label = "Charlie" }
        });
    }

    public Task<IEnumerable<Dropdown<int>>> GetProductsList()
    {
        return Task.FromResult<IEnumerable<Dropdown<int>>>(new List<Dropdown<int>>
        {
            new() { Id = 1, Label = "Laptop" },
            new() { Id = 2, Label = "Monitor" },
            new() { Id = 3, Label = "Keyboard" }
        });
    }

    public Task<IEnumerable<Dropdown<int>>> GetStockHolderList()
    {
        return Task.FromResult<IEnumerable<Dropdown<int>>>(new List<Dropdown<int>>
        {
            new() { Id = 1, Label = "Shareholder A" },
            new() { Id = 2, Label = "Shareholder B" },
            new() { Id = 3, Label = "Shareholder C" }
        });
    }
}