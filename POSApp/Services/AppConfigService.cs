using System;
using System.IO;
using POSApp.Models;

namespace POSApp.Services;

public class AppConfigService
{
    private const string FilePath = "Config.ini";

    public string ApplicationName { get; } = "eduPharmaManagerEnterprise";

    public string Server { get; private set; } = "";
    public string Database { get; private set; } = "";
    public string UserName { get; private set; } = "";
    public string Password { get; private set; } = "";
    public string ConnectionString { get; private set; } = null!;

    public bool Load()
    {
        try
        {
            using var fs = File.OpenRead(Path.Combine(Path.GetDirectoryName(Environment.ProcessPath)!, FilePath));
            using var reader = new StreamReader(fs);

            var server = reader.ReadLine();
            if (string.IsNullOrEmpty(server)) return false;
            Server = server;

            var database = reader.ReadLine();
            if (string.IsNullOrEmpty(database)) return false;
            Database = database;

            var username = reader.ReadLine();
            if (string.IsNullOrEmpty(username)) return false;
            UserName = username;

            var password = reader.ReadLine();
            if (string.IsNullOrEmpty(password)) return false;
            Password = password;

            ConnectionString = BuildConnectionString(server, database, username, password);
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

    public string BuildConnectionString(string server, string database, string username, string password)
    {
        return
            $"Data Source={server};initial Catalog={database};Integrated Security=false;User Id={username};Password={password};Max Pool Size=32767;";
    }
}