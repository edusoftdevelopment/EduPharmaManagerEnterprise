using System.Collections.Generic;
using System.Threading.Tasks;

namespace POSApp.Services;

public class Dropdown<T>
{
    public required T Id { get; set; }
    public required string Label { get; set; }
}

public class DropdownService
{
    
    public Task<IEnumerable<Dropdown<string>>> GetDefaultDatabases()
    {
        return Task.FromResult<IEnumerable<Dropdown<string>>>(new List<Dropdown<string>>
        {
            new() { Id = "Database", Label = "Database" },
            new() { Id = "Database", Label = "Database" },
            new() { Id = "Database", Label = "Database" }
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
