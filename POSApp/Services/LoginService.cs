using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using POSApp.Helpers;
using POSApp.Models;

namespace POSApp.Services;

public interface ILoginService
{
    Task<AppUser> Login(string username, string password);
}

public class LoginService(DbHelper dbHelper) : ILoginService
{
    public async Task<AppUser> Login(string username, string password)
    {
        var appUser = new AppUser();

        if (username == "admin" && password == "bismillah")
        {
            
        }
        else
        {
            throw new Exception("Invalid username or password");
        }
        
        // await dbHelper.ExecuteNonQueryAsync("", new SqlParameter("username", username), new SqlParameter("password", password));
         return appUser;
        
    }
}