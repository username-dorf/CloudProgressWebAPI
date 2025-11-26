using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthService.Data;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using LoginRequest = AuthService.Data.LoginRequest;

namespace AuthService.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly int ACCESS_TOKEN_EXPIRY_MINUTES = 60;
    private readonly int REFRESH_TOKEN_EXPIRY_DAYS = 30;
    private readonly ApplicationDbContext _dbContext;
    private readonly IConfiguration _configuration;

    public AuthController(ApplicationDbContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }

        var accessToken = GenerateJwtToken(user.Id);

        var refreshToken = Guid.NewGuid().ToString();
        var refreshTokenExpiresAt = GetRefreshTokenExpiryDate();

        var existingToken = await _dbContext.UserTokens
            .FirstOrDefaultAsync(t => t.UserId == user.Id && t.DeviceId == request.DeviceId);

        if (existingToken != null)
        {
            existingToken.RefreshToken = refreshToken;
            existingToken.ExpiresAt = refreshTokenExpiresAt;
        }
        else
        {
            var userToken = new UserToken
            {
                UserId = user.Id,
                DeviceId = request.DeviceId,
                RefreshToken = refreshToken,
                ExpiresAt = refreshTokenExpiresAt
            };

            _dbContext.UserTokens.Add(userToken);
        }

        await _dbContext.SaveChangesAsync();

        return Ok(new
        {
            access_token = accessToken,
            refresh_token = refreshToken
        });
    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var userToken = await _dbContext.UserTokens
            .FirstOrDefaultAsync(t => t.RefreshToken == request.RefreshToken && t.DeviceId == request.DeviceId);

        if (userToken == null || userToken.ExpiresAt < DateTime.UtcNow)
        {
            return Unauthorized(new { message = "Invalid or expired refresh token" });
        }

        var newAccessToken = GenerateJwtToken(userToken.UserId);
        var newRefreshToken = Guid.NewGuid().ToString();
        var newRefreshTokenExpiresAt = GetRefreshTokenExpiryDate();

        userToken.RefreshToken = newRefreshToken;
        userToken.ExpiresAt = newRefreshTokenExpiresAt;

        await _dbContext.SaveChangesAsync();

        return Ok(new
        {
            access_token = newAccessToken,
            refresh_token = newRefreshToken
        });
    }


    private DateTime GetRefreshTokenExpiryDate()
    {
        return DateTime.UtcNow.AddDays(REFRESH_TOKEN_EXPIRY_DAYS);
    }

    private string GenerateJwtToken(int userId)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException()));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(ACCESS_TOKEN_EXPIRY_MINUTES),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
