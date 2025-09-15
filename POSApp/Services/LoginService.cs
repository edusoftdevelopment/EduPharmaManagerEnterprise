using System;
using System.Diagnostics;
using System.Threading.Tasks;
using POSApp.Helpers;
using POSApp.Models;

namespace POSApp.Services;

public interface ILoginService
{
    Task<AppUser?> Login(string username, string password);
    Task CheckConnectionAsyc();
}

public class LoginService(DbHelper db) : ILoginService
{
    
    
    public async Task<AppUser?> Login(string username, string password)
    {
        var users = await db.ExecuteQueryAsync<AppUser>("SELECT * FROM LoginInfo", r => new AppUser
        {
            LoginId = r.GetInt32(r.GetOrdinal("LoginId")),
            LoginName = r.GetString(r.GetOrdinal("LoginName")),
            Password = r.GetString(r.GetOrdinal("Password")),
            LoginType = r.GetString(r.GetOrdinal("LoginType")),
            BackSessionWorkingAllowed = r.GetBoolean(r.GetOrdinal("BackSessionWorkingAllowed")),
            BackDateWorkingAllowed = r.IsDBNull(r.GetOrdinal("BackDateWorkingAllowed")) 
                ? false
                : r.GetBoolean(r.GetOrdinal("BackDateWorkingAllowed")),
            DefaultBusinessUnitID = r.IsDBNull(r.GetOrdinal("DefaultBusinessUnitID")) 
                ? 0 
                : r.GetInt32(r.GetOrdinal("DefaultBusinessUnitID")),
        });

        
        foreach (var user in users)
        {
            if (EncryptionHelper.eduDecrypt(user.LoginName) == username &&
                EncryptionHelper.eduDecrypt(user.Password) == password)
            {
                return user;    
            }
        }
        
        return null;
    }

    public async Task CheckConnectionAsyc()
    {
        await db.CheckConnectionAsync();
    }
}