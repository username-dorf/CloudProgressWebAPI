namespace AuthService.Data;

public class RefreshTokenRequest
{
    public string RefreshToken { get; set; }
    public string DeviceId { get; set; }
}