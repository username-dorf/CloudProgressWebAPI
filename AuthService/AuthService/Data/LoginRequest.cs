namespace AuthService.Data;

public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string DeviceId { get; set; } 
}