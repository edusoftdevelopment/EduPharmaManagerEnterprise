using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using POSApp.Services;
using POSApp.ViewModels;

namespace POSApp.Helpers;

public class DbHelper(AppConfigService appConfigService)
{
    public IEnumerable<T> ExecuteDefaultDbQuery<T>(
        string sql,
        Func<IDataReader, T> map,
        params SqlParameter[] parameters
        )
    {
        using var conn = new SqlConnection(appConfigService.DefaultConnectionString);
        using var cmd = new SqlCommand(sql, conn);
        if (parameters?.Length > 0) cmd.Parameters.AddRange(parameters);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        var list = new List<T>();
        while (reader.Read())
        {
            list.Add(map(reader));
        }
        return list;
    }
    
    public Task<IEnumerable<T>> ExecuteDefaultDbQueryAsync<T>(
        string sql,
        Func<IDataReader, T> map,
        params SqlParameter[] parameters
    )
    {
        return Task.Run(() =>
        {
            using var conn = new SqlConnection(appConfigService.DefaultConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            if (parameters?.Length > 0) cmd.Parameters.AddRange(parameters);
            conn.Open();
            using var reader = cmd.ExecuteReader();
            var list = new List<T>();
            
            while (reader.Read())
            {
                list.Add(map(reader));
            }

            return list.AsEnumerable();
        });
    }
    
    public IEnumerable<T> ExecuteQuery<T>(
        string sql,
        Func<IDataReader, T> map,
        params SqlParameter[] parameters
    )
    {
        using var conn = new SqlConnection(appConfigService.ConnectionString);
        using var cmd = new SqlCommand(sql, conn);
        if (parameters?.Length > 0) cmd.Parameters.AddRange(parameters);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        var list = new List<T>();
        while (reader.Read())
        {
            list.Add(map(reader));
        }
        return list;
    }
    
    public Task<IEnumerable<T>> ExecuteQueryAsync<T>(
        string sql,
        Func<IDataReader, T> map,
        params SqlParameter[] parameters
    )
    {
        return Task.Run(() =>
        {
            using var conn = new SqlConnection(appConfigService.ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            if (parameters?.Length > 0) cmd.Parameters.AddRange(parameters);
            conn.Open();
            using var reader = cmd.ExecuteReader();
            var list = new List<T>();
            while (reader.Read())
            {
                list.Add(map(reader));
            }
            return list.AsEnumerable();
        });
    }

    public Task CheckConnectionAsync()
    {
        return Task.Run(() =>
        {
            using var conn = new SqlConnection(appConfigService.DefaultConnectionString);
            conn.Open();
        });
    }
}