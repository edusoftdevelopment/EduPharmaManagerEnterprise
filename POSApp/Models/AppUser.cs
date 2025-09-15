namespace POSApp.Models;

public class AppUser
{
    public required int LoginId { get; set; }
    public required string LoginName { get; set; }
    public required string Password { get; set; }
    public required string LoginType { get; set; }
    public required bool BackSessionWorkingAllowed { get; set; }
    public required bool BackDateWorkingAllowed { get; set; }
    public required int DefaultBusinessUnitID { get; set; } 
}