using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RepositoryService.Data;
using RepositoryService.Repository;

namespace RepositoryService.Controllers;

[ApiController]
[Route("api/progress")]
public class ProgressController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly UserProgressRepository _progressRepository;
    private readonly ILogger<ProgressController> _logger;

    public ProgressController(UserProgressRepository userProgressRepository, IConfiguration configuration,
        ILogger<ProgressController> logger)
    {
        _logger = logger;
        _progressRepository = userProgressRepository;
        _configuration = configuration;
    }

    [HttpPost("save")]
    public async Task<IActionResult> SaveProgress([FromBody] SaveProgressRequest request)
    {
        var accessToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        if (!TryValidateToken(accessToken, out int userId))
        {
            return Unauthorized(new { message = "Invalid or expired access token" });
        }

        string jsonProgress;
        try
        {
            jsonProgress = JsonSerializer.Serialize(request.Progress);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Invalid progress format.", error = ex.Message });
        }

        var userProgress = new UserProgress
        {
            UserId = userId,
            DeviceId = request.DeviceId,
            ProgressJson = jsonProgress,
            AppVersion = request.AppVersion,
            LastUpdated = DateTime.UtcNow
        };

        await _progressRepository.SaveProgressAsync(userProgress);

        return Ok(new { message = "Progress saved successfully", lastDevice = request.DeviceId });
    }


    [HttpPost("load")]
    public async Task<IActionResult> LoadProgress([FromBody] LoadProgressRequest request)
    {
        var accessToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        if (!TryValidateToken(accessToken, out int userId))
        {
            return Unauthorized(new { message = "Invalid or expired access token" });
        }

        var userProgress = await _progressRepository.GetProgressAsync(userId, request.DeviceId);
        if (userProgress == null)
        {
            return NotFound(new { message = "Progress not found" });
        }

        return Ok(new
        {
            userId = userProgress.UserId,
            lastDevice = userProgress.DeviceId,
            progress = JsonSerializer.Deserialize<object>(userProgress.ProgressJson),
            lastUpdated = userProgress.LastUpdated
        });
    }

    
    private bool TryValidateToken(string token, out int userId)
    {
        userId = 0;
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var validationParams = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            var principal = tokenHandler.ValidateToken(token, validationParams, out SecurityToken validatedToken);

            var userIdClaim = principal.FindFirst("sub");
            if (userIdClaim != null)
            {
                userId = int.Parse(userIdClaim.Value);
                return true;
            }
        }
        catch (SecurityTokenExpiredException ex)
        {
            _logger.LogError(ex, "Token validation failed: token is expired.");
        }
        catch (SecurityTokenInvalidIssuerException ex)
        {
            _logger.LogError(ex, $"Token validation failed: invalid issuer. Expected issuer: {_configuration["Jwt:Issuer"]}");
        }
        catch (SecurityTokenInvalidAudienceException ex)
        {
            _logger.LogError(ex, $"Token validation failed: invalid audience. Expected audience: {_configuration["Jwt:Audience"]}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Token validation failed due to an unexpected error.");
        }

        return false;
    }
}


