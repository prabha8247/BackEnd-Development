using System;

namespace Employee.Models;

public class User
{
    public string Id { get; set; }
    public string FName { get; set; }
    public string LName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime CreatedTime { get; set; }
    public string Username { get; set; }
    public string Role { get; set; }
    
}