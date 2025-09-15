using System;
using System.IO;
using POSApp.Models;

namespace POSApp.Services;

public class AppConfigService
{
    private const string FilePath = "Config.ini";

    public string ApplicationName { get; } = "eduPharmaManagerEnterprise";

    private string Server { get; set; } = "";
    public string Database { get; private set; } = "";
    private string UserName { get; set; } = "";
    private string Password { get; set; } = "";
    public string DefaultConnectionString { get; private set; } = null!;
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

            DefaultConnectionString = BuildConnectionString(database);
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
    public void BuildAppConnectionString(string database)
    {
        ConnectionString = BuildConnectionString(database);
    }

    private string BuildConnectionString(string database)
    {
        return $"Data Source=tcp:{Server};initial Catalog={database};Integrated Security=false;User Id={UserName};Password={Password};Max Pool Size=32767;Connection Timeout=10;Encrypt=False;TrustServerCertificate=True;";
    }
}