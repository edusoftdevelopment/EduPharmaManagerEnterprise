using System;
using System.IO;
using POSApp.Models;

namespace POSApp.Services;

public class AppConfigService
{
    private const string FilePath = "Config.ini";
    public string ConnectionString { get; private set; } = null!;
    
    public bool Load()
    {
        try
        {
            using var fs = File.OpenRead(Path.Combine(Path.GetDirectoryName(Environment.ProcessPath)!, FilePath));
            using var reader = new StreamReader(fs);
            var connectionString = reader.ReadLine();
            if (string.IsNullOrEmpty(connectionString)) return false;
            ConnectionString = connectionString;
            return true;
        }
        catch (Exception e)
        {
            if (e is FileNotFoundException)
            {
            }
            return false;
        }
    }
}