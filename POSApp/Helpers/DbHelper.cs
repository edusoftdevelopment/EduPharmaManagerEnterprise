using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace POSApp.Helpers;

public class DbHelper
{
    private string _connectionString = string.Empty;

    public void SetConnectionString(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public async Task<DataTable> ExecuteQueryAsync(string sql, params SqlParameter[] parameters)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(sql, conn);
        if (parameters != null) cmd.Parameters.AddRange(parameters);
        var dt = new DataTable();
        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();
        dt.Load(reader);
        return dt;
    }
    
    public async Task<List<T>> ExecuteQueryAsync<T>(
        string sql,
        Func<IDataReader, T> map,
        params SqlParameter[] parameters)
    {
        
        await using var conn = new SqlConnection(_connectionString);
        await using var cmd = new SqlCommand(sql, conn);
        if (parameters?.Length > 0) cmd.Parameters.AddRange(parameters);

        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        await conn.OpenAsync(cts.Token);
        
        await using var reader = await cmd.ExecuteReaderAsync(cts.Token);
        var list = new List<T>();
        while (await reader.ReadAsync(cts.Token))
        {
            list.Add(map(reader));
        }
        return list;
    }
    
    public async Task<int> ExecuteNonQueryAsync(string sql, params SqlParameter[] parameters)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(sql, conn);
        if (parameters != null) cmd.Parameters.AddRange(parameters);
        await conn.OpenAsync();
        return await cmd.ExecuteNonQueryAsync();
    }
    
    public async Task<object?> ExecuteScalarAsync(string sql, params SqlParameter[] parameters)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(sql, conn);
        if (parameters != null) cmd.Parameters.AddRange(parameters);
        await conn.OpenAsync();
        return await cmd.ExecuteScalarAsync();
    }
    
    public async Task<int> ExecuteStoredProcAsync(
        string procName, params SqlParameter[] parameters)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(procName, conn);
        cmd.CommandType = CommandType.StoredProcedure;
        if (parameters != null) cmd.Parameters.AddRange(parameters);

        await conn.OpenAsync();
        return await cmd.ExecuteNonQueryAsync();
    }

    public async Task<object?> ExecuteStoredProcScalarAsync(
        string procName, params SqlParameter[] parameters)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(procName, conn);
        cmd.CommandType = CommandType.StoredProcedure;
        if (parameters != null) cmd.Parameters.AddRange(parameters);

        await conn.OpenAsync();
        return await cmd.ExecuteScalarAsync();
    }
    
    

}